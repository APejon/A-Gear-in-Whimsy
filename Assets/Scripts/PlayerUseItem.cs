using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private UIResourceManager _UI;
    private Animator _animator;
    private PlayerStats _playerStats;
    private float health;
    private Inventory _inventory;
    private Coroutine _Use;
    private bool _isUsing;
    public Material flashMaterial;
    private Material originalMaterial;
    private SkinnedMeshRenderer childSkinRenderer;
    private MeshRenderer childRenderer;

    void Start()
    {
        _UI = UIResourceManager.Instance;
        _animator = GetComponent<Animator>();
        _playerStats = PlayerStats.Instance;
        _inventory = Inventory.Instance;
        _isUsing = false;
        childSkinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        childRenderer = GetComponentInChildren<MeshRenderer>();
        if (childSkinRenderer)
            originalMaterial = childSkinRenderer.material;
        else if (childRenderer)
            originalMaterial = childRenderer.material;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || health != _playerStats.health)
        {
            // If player moves while using an item, cancel the animation process
            if (_isUsing)
            {
                StopCoroutine(_Use);
                _animator.SetBool("Item", false);
                _isUsing = false;
                _playerStats.Equip(1);
                _playerStats.Unequip(4);
                _UI.useNotif("Item use interrupted...", UIResourceManager.notifType.INTERUPTTED);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerStats.CheckCrystal() == null)
                _UI.useNotif("Don't have a crystal!", UIResourceManager.notifType.WARNING);
            else if (_playerStats.health == 5 && _playerStats.CheckCrystal().Effect == ConsumableType.Heal)
                _UI.useNotif("Health is already full!", UIResourceManager.notifType.WARNING);
            else
            {
                health = _playerStats.health;
                _Use = StartCoroutine(UseItemCoroutine());
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _playerStats.switchItem(1);
                _UI.useArrows(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _playerStats.switchItem(2);
                _UI.useArrows(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _playerStats.switchItem(3);
                _UI.useArrows(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _playerStats.switchItem(4);
                _UI.useArrows(4);
            }
        }
    }

    IEnumerator UseItemCoroutine()
    {
        _playerStats.Unequip(1);
        _playerStats.Equip(4);
        _isUsing = true;
        _animator.SetBool("Item", true);

        yield return new WaitForSeconds(2f);

        _playerStats.Equip(1);
        _playerStats.Unequip(4);
        ConsumableEffect();
        _UI.updateUI();
        _isUsing = false;
        _animator.SetBool("Item", false);
    }

    IEnumerator HealFlash()
    {
        if (childSkinRenderer)
            childSkinRenderer.material = flashMaterial;
        else if (childRenderer)
            childRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.5f);
        if (childSkinRenderer)
            childSkinRenderer.material = originalMaterial;
        else if (childRenderer)
            childRenderer.material = originalMaterial;
    }

    private void ConsumableEffect()
    {
        if (_playerStats.getEffect() == ConsumableType.Heal)
        {
            StartCoroutine(HealFlash());
            _playerStats.health++;
            _playerStats.CheckCrystal().DurabilityDecrease();
            _playerStats.checkEquipped();
            _inventory.checkInventory();
        }
    }
}
