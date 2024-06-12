using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    private Weapon _equippedWeapon;
    private Tool _equippedWoodTool;
    private Tool _equippedStoneTool;
    private Consumable _equippedconsumable;
    private GameObject _weapon;
    private GameObject _woodTool;
    private GameObject _stoneTool;
    private GameObject _consumable;
    public Transform handAttachPoint;
    public Transform attackPoint;
    public float attackRange;

    public enum State { Safe, Combat }
    private State _currentState = State.Safe;
    private bool _invulnerable = false;
    private static PlayerStats _instance;
    public static PlayerStats Instance { get { return _instance; } }
    private Rigidbody _playerRigidBody;

    private int _enemiesDetected = 0;
    private EnemyStats _enemy;

    private SkinnedMeshRenderer childSkinRenderer;
    private MeshRenderer childRenderer;
    public Material flashMaterial;
    private Material originalMaterial;
    public Material pulseMaterial;
    private Inventory _bag;
    private UIResourceManager _UI;
    private PlayerHarvest _playerHarvest;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        _playerRigidBody = gameObject.GetComponent<Rigidbody>();
        childSkinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        childRenderer = GetComponentInChildren<MeshRenderer>();
        if (childSkinRenderer)
            originalMaterial = childSkinRenderer.material;
        else if (childRenderer)
            originalMaterial = childRenderer.material;
    }

    private void Start()
    {
        _bag = Inventory.Instance;
        _UI = _UI = UIResourceManager.Instance;
        _playerHarvest = GetComponent<PlayerHarvest>();
    }

    public void FirstEquip()
    {
        List<Item> itemList;
        Tool tooltemp;

        itemList = Inventory.Instance.GetAllItems();
        foreach (Item item in itemList)
        {
            if (item.Type == ItemType.Weapon && CheckWeapon() == null)
            {
                _equippedWeapon = (Weapon)item;
                _weapon = item.Mesh;
                // Instantiate a new wooden sword GameObject
                // _weapon = Instantiate(item.Mesh, handAttachPoint.position, handAttachPoint.rotation);
                // Parent the wooden sword to the player's hand attachment point
                if (handAttachPoint != null && getStyle() == WeaponType.Sword)
                {
                    _weapon.transform.parent = handAttachPoint;
                    _weapon.transform.localPosition = new Vector3(0f, 0.071f, 0f); // Optional: Adjust position relative to attachment point
                    _weapon.transform.localRotation = Quaternion.Euler(new Vector3(-69f, 46f, 20f)); // Optional: Adjust rotation relative to attachment point
                    _weapon.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); // Optional: Adjust Scale relative to attachment point
                    attackPoint.localPosition = new Vector3(-0.01f, 0.77f, 1.01f);
                }
                continue ;
            }
            if (item.Type == ItemType.Tool)
            {
                tooltemp = (Tool)item;
                if (getMaterial(tooltemp) == HarvestType.Wood && CheckWood() == null)
                {
                    _equippedWoodTool = tooltemp;
                    _woodTool = item.Mesh;
                    // _woodTool = Instantiate(item.Mesh, handAttachPoint.position, handAttachPoint.rotation);
                if (handAttachPoint != null)
                {
                    _woodTool.transform.parent = handAttachPoint;
                    _woodTool.transform.localPosition = new Vector3(0.6f, 0.12f, -0.22f); // Optional: Adjust position relative to attachment point
                    _woodTool.transform.localRotation = Quaternion.Euler(new Vector3(74f, -188f, -296f)); // Optional: Adjust rotation relative to attachment point
                    _woodTool.transform.localScale = new Vector3(8f, 8f, 8f); // Optional: Adjust Scale relative to attachment point
                    _woodTool.SetActive(false);
                }
                }
                else if (getMaterial(tooltemp) == HarvestType.Stone && CheckStone() == null)
                {
                    _equippedStoneTool = tooltemp;
                    _stoneTool = item.Mesh;
                    // _stoneTool = Instantiate(item.Mesh, handAttachPoint.position, handAttachPoint.rotation);
                if (handAttachPoint != null)
                {
                    _stoneTool.transform.parent = handAttachPoint;
                    _stoneTool.transform.localPosition = new Vector3(0.14f, 0.17f, -0.05f); // Optional: Adjust position relative to attachment point
                    _stoneTool.transform.localRotation = Quaternion.Euler(new Vector3(-50f, 101f, 6f)); // Optional: Adjust rotation relative to attachment point
                    _stoneTool.transform.localScale = new Vector3(4f, 4f, 4f); // Optional: Adjust Scale relative to attachment point
                    _stoneTool.SetActive(false);
                }
                }
                continue ;
            }
            if (item.Type == ItemType.Consumable && CheckCrystal() == null)
            {
                _equippedconsumable = (Consumable)item;
                _consumable = item.Mesh;
                // _consumable = Instantiate(item.Mesh, handAttachPoint.position, handAttachPoint.rotation);
                if (handAttachPoint != null)
                {
                    _consumable.transform.parent = handAttachPoint;
                    _consumable.transform.localPosition = new Vector3(0.02f, 0.068f, 0.035f); // Optional: Adjust position relative to attachment point
                    _consumable.transform.localRotation = Quaternion.Euler(new Vector3(11.3f, 35f, 87f)); // Optional: Adjust rotation relative to attachment point
                    _consumable.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Optional: Adjust Scale relative to attachment point
                    _consumable.SetActive(false);
                }
                continue ;
            }
        }
        _UI.updateEquipped();
    }

    public void Equip(int Choice)
    {
        if (Choice == 1 && CheckWeapon() != null)
            _weapon.SetActive(true);
        else if (Choice == 2 && CheckWood() != null)
            _woodTool.SetActive(true);
        else if (Choice == 3 && CheckStone() != null)
            _stoneTool.SetActive(true);
        else if (Choice == 4 && CheckCrystal() != null)
            _consumable.SetActive(true);

    }

    public void Unequip(int Choice)
    {
        if (Choice == 1 && CheckWeapon() != null)
            _weapon.SetActive(false);
        else if (Choice == 2 && CheckWood() != null)
            _woodTool.SetActive(false);
        else if (Choice == 3 && CheckStone() != null)
            _stoneTool.SetActive(false);
        else if (Choice == 4 && CheckCrystal() != null)
            _consumable.SetActive(false);
    }

    public void SetPlayerState(State state)
    {
        if (state == State.Combat)
        {
            _enemiesDetected++;
            _currentState = state;
            UIPlayerCondition.Instance.ShowCombatImage();
        }
        else if (state == State.Safe)
        {
            _enemiesDetected--;
            if (_enemiesDetected == 0)
            {
                _currentState = state;
                UIPlayerCondition.Instance.HideCombatImage();
            }
        }
    }

    public bool SafeMode()
    {
        return _currentState == State.Safe;
    }

    public bool CombatMode()
    {
        return _currentState == State.Combat;
    }
    
    public void Vulnerabilize()
    {
        _invulnerable = false;
    }

    IEnumerator InvulnerablePulse()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            if (childSkinRenderer)
                childSkinRenderer.material = pulseMaterial;
            else if (childRenderer)
                childRenderer.material = pulseMaterial;
            yield return new WaitForSeconds(0.2f);
            if (childSkinRenderer)
                childSkinRenderer.material = originalMaterial;
            else if (childRenderer)
                childRenderer.material = originalMaterial;
            yield return new WaitForSeconds(0.2f);
            elapsedTime += 0.4f;
        }
    }

    IEnumerator DamageFlash(Vector3 hitDirection)
    {
        if (childSkinRenderer)
            childSkinRenderer.material = flashMaterial;
        else if (childRenderer)
            childRenderer.material = flashMaterial;
        _playerRigidBody.AddForce((hitDirection * 3) + (Vector3.up * 3), ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        if (this.health > 0)
            StartCoroutine(InvulnerablePulse());
        else
        {
            if (childSkinRenderer)
                childSkinRenderer.material = originalMaterial;
            else if (childRenderer)
                childRenderer.material = originalMaterial;
        }
    }

    public void TakeDamage(Vector3 damageDirection)
    {
        if (!_invulnerable)
        {
            StartCoroutine(DamageFlash(damageDirection.normalized));
            health--;
            _invulnerable = true;
            Invoke("Vulnerabilize", 5.0f);
            _UI.updateUI();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemy = other.gameObject.GetComponent<EnemyStats>(); 
            if (_enemy.currentHealth != 0)
                this.TakeDamage(other.transform.forward);
        }
    }

    public Weapon CheckWeapon()
    {
        if (_equippedWeapon != null)
            return _equippedWeapon;
        return null;
    }
    
    public Tool CheckWood()
    {
        if (_equippedWoodTool != null)
            return _equippedWoodTool;
        return null;
    }

    public Tool CheckStone()
    {
        if (_equippedStoneTool != null)
            return _equippedStoneTool;
        return null;
    }

    public Consumable CheckCrystal()
    {
        if (_equippedconsumable != null)
            return _equippedconsumable;
        return null;
    }

    public WeaponType getStyle()
    {
        return _equippedWeapon.Style;
    }

    public HarvestType getMaterial(Tool tooltemp)
    {
        return tooltemp.Material;
    }

    public ConsumableType getEffect()
    {
        return _equippedconsumable.Effect;
    }

    public void checkEquipped()
    {
        if (_equippedWeapon != null && _equippedWeapon.Durability == 0)
        {
            _weapon.SetActive(false);
            _equippedWeapon = null;
        }
        if (_equippedWoodTool != null && _equippedWoodTool.Durability == 0)
            _equippedWoodTool = null;
        if (_equippedStoneTool != null && _equippedStoneTool.Durability == 0)
            _equippedStoneTool = null;
        if (_equippedconsumable != null && _equippedconsumable.Durability == 0)
            _equippedconsumable = null;
        _UI.updateEquipped();
    }

    public void switchItem(int buttonPressed)
    {
        if (buttonPressed == 1)
            _equippedWeapon = (Weapon)_bag.findNext(_equippedWeapon, ItemType.Weapon, HarvestType.None);
        else if (buttonPressed == 2)
            _equippedWoodTool = (Tool)_bag.findNext(_equippedWoodTool, ItemType.Tool, HarvestType.Wood);
        else if (buttonPressed == 3)
            _equippedStoneTool = (Tool)_bag.findNext(_equippedStoneTool, ItemType.Tool, HarvestType.Stone);
        else if (buttonPressed == 4)
            _equippedconsumable = (Consumable)_bag.findNext(_equippedconsumable, ItemType.Consumable, HarvestType.None);
        _UI.updateEquipped();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
