using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerAttack : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _style;
    private PlayerController _controls;
    private int _comboCount = 0;
    private float _comboResetTime = 1f;
    private bool _canAttack = true;
    private float _attackCooldown = 0.7f;
    private float _lastAttackTime = 0f;
    private float _idleTime = 3.0f;
    public LayerMask enemyLayers;

    private PlayerStats _playerStats;
    private Inventory _inventory;
    private UIResourceManager _UI;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _style = GetComponent<Animator>();
        _controls = GetComponent<PlayerController>();
        _playerStats = PlayerStats.Instance;
        _inventory = Inventory.Instance;
        _UI = UIResourceManager.Instance;
    }

    void Update()
    {
        if (_playerStats.health > 0)
        {
            // Idle Animation
            if (Time.time - _lastAttackTime >= _idleTime && _playerStats.CheckWeapon() != null)
            {
                if (_playerStats.getStyle() == WeaponType.Sword)
                    _style.SetBool("Swordidle", true);
            }
            // Reset Combo
            if (Time.time - _lastAttackTime > _comboResetTime)
            {
                _comboCount = 0;
            }

            // Prevent attack spamming
            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                _canAttack = true;
                _controls.enabled = true;
            }
            // Check if weapon equipped before attacking
            if (_playerStats.CheckWeapon() != null)
            {
                if (Input.GetButtonDown("Fire1") && _canAttack && _playerStats.getStyle() == WeaponType.Sword)
                {
                    _controls.enabled = false;
                    Attack();
                }
            }
            else if (Input.GetButtonDown("Fire1"))
                _UI.useNotif("Don't have a sword!", UIResourceManager.notifType.WARNING);
        }
    }

    void Attack()
    {
        _style.SetBool("Swordidle", false);
        _comboCount++;
        ResetAttack();

        switch (_comboCount)
        {
            case 1:
                _style.SetTrigger("Attack1");
                break ;
            case 2:
                _rb.velocity = transform.forward * -7;
                _style.SetTrigger("Attack2");
                break ;
            case 3:
                _rb.velocity = transform.forward * 7;
                _style.SetTrigger("Attack3");
                _comboCount = 0;
                break ;
            default:
                _comboCount = 0;
                break ;
        }
        Collider[] hitEnemies = Physics.OverlapSphere(_playerStats.attackPoint.position, 1.0f, enemyLayers);
        foreach(Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyStats>().currentHealth > 0)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(_playerStats.CheckWeapon().Damage, gameObject.transform.forward);
                _playerStats.CheckWeapon().DurabilityDecrease();
                _playerStats.checkEquipped();
                _inventory.checkInventory();
            }
        }
        _canAttack = false;
    }

    public void ResetAttack()
    {
        _lastAttackTime = Time.time;
    }

}
