using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    private float _moveSpeed = 4f;
    private float _rotationSpeed = 500f;

    private Rigidbody _rb;
    private float _jumpForce = 4f;
    private bool _isOnGround = true;

    private bool _isDashing = false;
    private bool _isSprinting = false;
    public float dashSpeed = 10.0f;
    public float dashCooldown = 1.0f;

    private Animator _animator;
    private bool running = false;
    private PlayerStats _plStats;
    private PlayerAttack _plAttack;
    private UIResourceManager _UI;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _plStats = PlayerStats.Instance;
        _plAttack = GetComponent<PlayerAttack>();
        _UI = UIResourceManager.Instance;
    }

    void Update()
    {
        if (_plStats.health <= 0)
        {
            _animator.SetTrigger("Death");
            this.enabled = false;
            _plStats.enabled = false;
            _UI.gameOver();
        }
        else
        {
            // Input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 cameraFoward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraFoward.y = 0;
            cameraRight.y = 0;

            cameraFoward.Normalize();
            cameraRight.Normalize();

            // Movement vector
            // Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
            Vector3 movement = (cameraRight * moveX + cameraFoward * moveZ).normalized;

            // Rotation
            if (movement != Vector3.zero)
            {
                _animator.SetBool("Swordidle", false);
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.Space) && _isOnGround == true)
            {
                _animator.SetTrigger("Jump");
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _isOnGround = false;
            }
            if (Input.GetKeyDown(KeyCode.V) && _isDashing == false)
                Dash();
            if (Input.GetKeyDown(KeyCode.LeftShift) && _isDashing == false)
            {
                // Sprint
                if (_plStats.SafeMode())
                {
                    if (_isSprinting == false)
                    {
                        _isSprinting = true;
                        if (running)
                            _animator.SetBool("Sprinting", true);
                        _animator.SetBool("Running", false);
                        _moveSpeed *= 3f;
                    }
                    else if (_isSprinting == true)
                    {
                        _isSprinting = false;
                        if (running)
                            _animator.SetBool("Running", true);
                        _animator.SetBool("Sprinting", false);
                        _moveSpeed /= 3f;
                    }
                }
            }
            else if (_plStats.CombatMode())
            {
                if (_isSprinting == true)
                {
                    _isSprinting = false;
                    if (running)
                        _animator.SetBool("Running", true);
                    _animator.SetBool("Sprinting", false);
                    _moveSpeed /= 3f;
                }
            }
            
        }
    }

    void FixedUpdate()
    {
        if (_plStats.health > 0)
        {
            // Movement
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveZ) > 0.01f)
            {
                _plAttack.ResetAttack();
                Vector3 cameraFoward = cameraTransform.forward;
                Vector3 cameraRight = cameraTransform.right;

                cameraFoward.y = 0;
                cameraRight.y = 0;

                cameraFoward.Normalize();
                cameraRight.Normalize();

                // Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
                Vector3 movement = (cameraRight * moveX + cameraFoward * moveZ).normalized;
                _rb.MovePosition(_rb.position + movement * _moveSpeed * Time.fixedDeltaTime);
                if (!running)
                {
                    running = true;
                    if (_isSprinting)
                        _animator.SetBool("Sprinting", true);
                    else
                        _animator.SetBool("Running", true);
                }
            }
            else if (running)
            {
                running = false;
                if (_isSprinting)
                    _animator.SetBool("Sprinting", false);
                else
                    _animator.SetBool("Running", false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        _isOnGround = true;
    }

    void Dash()
    {
        _isDashing = true;
        _animator.SetTrigger("Dash");
        _rb.velocity = transform.forward * dashSpeed;
        Invoke("ResetDash", dashCooldown);
    }

    void ResetDash()
    {
        _isDashing = false;
    }
}