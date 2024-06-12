using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackType { Stomp, DishWack, DishThrow };
    public AttackType attackType;
    public float attackSpeed;
    public Transform player;

    private Vector3 _topOfPlayer;
    private Vector3 _stompLocation;
    private bool _stomp = false;
    private bool _dive = false;

    void Update()
    {
        if (attackType == AttackType.Stomp)
        {
            if (!_stomp && !_dive)
            {
                _topOfPlayer = player.transform.position + new Vector3(0, 5, 0);
                _stomp = true;
            }
            if (_stomp && !_dive)
            {
                Vector3 toTopOfPlayer = (_topOfPlayer - transform.position).normalized;
                transform.position += toTopOfPlayer * attackSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, _topOfPlayer) < 0.5f)
                {
                    _stomp = false;
                    _dive = true;
                    _stompLocation = player.position;
                }
            }
            if (_dive)
            {
                Vector3 directionToStomp = (_stompLocation - transform.position).normalized;
                transform.position += directionToStomp * attackSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, player.position) < 0.5f)
                    this.enabled = false;
            }
        }
    }

    void OnDisable()
    {
        _stomp = false;
        _dive = false;
    }
}
