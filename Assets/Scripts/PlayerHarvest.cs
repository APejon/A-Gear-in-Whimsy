using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    public float harvestDuration = 2f;

    private bool _isHarvesting = false;
    private GameObject _detected = null;
    private Coroutine _Harvest;

    private Animator _animator;
    private Inventory _inventory;
    private UIResourceManager _UI;
    private PlayerStats _playerStats;
    private float health;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _inventory = Inventory.Instance;
        _UI = UIResourceManager.Instance;
        _playerStats = PlayerStats.Instance;
    }

    // Update is called once per frame
   void Update()
    {
        // Check for player movement to interupt harvesting
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || health != _playerStats.health)
        {
            // If player moves while harvesting, cancel the harvesting process
            if (_isHarvesting)
            {
                StopCoroutine(_Harvest);
                _animator.SetBool("Harvest", false);
                _isHarvesting = false;
                _playerStats.Equip(1);
                _playerStats.Unequip(2);
                _playerStats.Unequip(3);
                _UI.useNotif("Harvest interrupted...", UIResourceManager.notifType.INTERUPTTED);
            }
        }

        GameObject harvestable = null;
        // Check for nearby harvestables
        if (_detected)
        {
            harvestable = ObjectPool.instance.GetPooledObject(_detected);
            if (harvestable && harvestable.GetComponent<Outline>() != null)
                harvestable.GetComponent<Outline>().enabled = true;
        }
        
        if (harvestable && !_isHarvesting && Input.GetKeyDown(KeyCode.F))
        {
            if (harvestable.name == "Wood(Clone)" && _playerStats.CheckWood() == null)
            {
                _UI.useNotif("Don't have an axe", UIResourceManager.notifType.WARNING);
                return ;
            }
            else if (harvestable.name == "Wood(Clone)")
            {
                _playerStats.Unequip(1);
                _playerStats.Equip(2);
            }
            if (harvestable.name == "Stone(Clone)" && _playerStats.CheckStone() == null)
            {
                _UI.useNotif("Don't have a pickaxe", UIResourceManager.notifType.WARNING);
                return ;
            }
            else if (harvestable.name == "Stone(Clone)")
            {
                _playerStats.Unequip(1);
                _playerStats.Equip(3);
            }
            health = _playerStats.health;
            _Harvest = StartCoroutine(HarvestAnimationCoroutine(harvestable));
        }
    }
    IEnumerator HarvestAnimationCoroutine(GameObject harvestable)
    {
        Vector3 direction = (harvestable.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        // Vector3 targetDirection = harvestable.transform.position - transform.position;
        // Debug.DrawRay(transform.position, targetDirection, Color.red, 5, true);

        _isHarvesting = true;
        _animator.SetBool("Harvest", true);

        yield return new WaitForSeconds(harvestDuration);

        _animator.SetBool("Harvest", false);
        harvestable.GetComponent<Outline>().enabled = false;
        harvestable.SetActive(false);
        if (harvestable.name == "Wood(Clone)")
        {
            _playerStats.Unequip(2);
            _playerStats.Equip(1);
            _playerStats.CheckWood().DurabilityDecrease();
            _playerStats.checkEquipped();
            _inventory.checkInventory();
            _inventory.AddWood(3);
        }
        else if (harvestable.name == "Stone(Clone)")
        {
            _playerStats.Unequip(3);
            _playerStats.Equip(1);
            _playerStats.CheckStone().DurabilityDecrease();
            _playerStats.checkEquipped();
            _inventory.checkInventory();
            _inventory.AddStone(3);
        }
        _UI.updateUI();
        _isHarvesting = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Harvestable"))
            _detected = other.gameObject;
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Harvestable"))
        {
            if (_detected && _detected.GetComponent<Outline>() != null)
                _detected.GetComponent<Outline>().enabled = false;
            _detected = null;
        }
    }

    public bool IsHarvesting()
    {
        if (_isHarvesting)
            return true;
        else
            return false;
    }
}
