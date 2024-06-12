using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private enum State { Patrol, Reset, Surprised, Chase, Attack }
    private Vector3 originalPosition;
    private Vector3 patrolPosition;
    private Vector3 Destination;
    private Vector3 resetPosition;
    private Quaternion originalRotation;
    private State currentState = State.Patrol;
    private PlayerStats _plStats;
    private EnemyAttack _enemyAttack;
    private EnemyStats _enemystats;

    public Transform player;
    public float proximity = 10f;
    public float attackRange = 3f;
    public float moveSpeed = 6f;
    public float patrolSpeed = 3f;

    private Vector3 startPosition;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        patrolPosition = originalPosition + (transform.forward * 20);
        originalRotation = transform.rotation;
        _plStats = PlayerStats.Instance;
        _enemyAttack = this.GetComponent<EnemyAttack>();
        _enemystats = this.GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the provided player exists
        if (player != null)
        {
            // Gets the distance and compares to the proximity threshold
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= proximity && currentState != State.Reset && _plStats.health > 0)
            {
                transform.LookAt(player);
                if (currentState == State.Patrol)
                {
                    // Just so enemies don't reset easily
                    proximity += 3f;
                    SwitchState(State.Surprised);
                }
                if (distance < attackRange && (currentState == State.Chase || currentState == State.Attack))
                    SwitchState(State.Attack);
                else
                {
                    // Ensures the position the enemy saw the player is the position it returns to when resetting
                    if (currentState == State.Surprised)
                    {
                        _plStats.SetPlayerState(PlayerStats.State.Combat);
                        resetPosition = transform.position;
                    }
                    SwitchState(State.Chase);
                }

            }
            else
            {
                float resetDistance = Vector3.Distance(transform.position, resetPosition);
                // Returns to reset position when player escapes
                if (resetDistance > 1f && currentState != State.Patrol)
                {
                    // Reset back proximity
                    if (currentState != State.Reset)
                    {
                        _plStats.SetPlayerState(PlayerStats.State.Safe);
                        proximity -= 3f;
                    }
                    SwitchState(State.Reset);
                }
                else if (resetDistance <= 1f || currentState == State.Patrol)
                {
                    SwitchState(State.Patrol);
                }
            }
        }
    }

    void SwitchState(State current)
    {
        currentState = current;
        switch (currentState)
        {
            case State.Patrol:
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, 500f * Time.deltaTime);
                float distance = Vector3.Distance(transform.position, originalPosition);
                float distance2 = Vector3.Distance(transform.position, patrolPosition);
                if (distance < 2f)
                    Destination = patrolPosition;
                else if (distance2 < 2f)
                    Destination = originalPosition;
                Vector3 directionOfPatrol = (Destination - transform.position).normalized;

                transform.LookAt(Destination);
                transform.position += directionOfPatrol * patrolSpeed * Time.deltaTime;

                break;
            case State.Reset:
                Vector3 directionToOrigin = (resetPosition - transform.position).normalized;

                transform.LookAt(resetPosition);
                transform.position += directionToOrigin * moveSpeed * Time.deltaTime;

                break;
            case State.Surprised:
                StartCoroutine(ShakeCoroutine());

                break;
            case State.Chase:
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

                break;
            case State.Attack:
                float distance3 = Vector3.Distance(transform.position, player.position);
                if (distance3 < attackRange - 1)
                {
                    Vector3 awayFromPlayer = (player.position - transform.position).normalized;

                    transform.position -= awayFromPlayer * moveSpeed * Time.deltaTime;

                }
                if (_enemyAttack)
                    StartCoroutine(AttackCoroutine());
                break;
        }
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        startPosition = transform.position;

        while (elapsedTime < shakeDuration)
        {
            //float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            //float offsetZ = Random.Range(-shakeIntensity, shakeIntensity);

            transform.position = startPosition + new Vector3(0, offsetY, 0);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        if (_enemystats.currentHealth != 0)
        {
            _enemyAttack.enabled = true;
            this.enabled = false;
        }
        else
            this.enabled = false;
        yield return new WaitForSeconds(2.0f);
        if (_enemystats.currentHealth != 0)
        {
            _enemyAttack.enabled = false;
            this.enabled = true;
        }
        else
            this.enabled = false;
    }
}
