using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    List<Item> itemList;
    private Animator _itemSpinner;
    private BoxCollider _collider;
    public GameObject extractedMesh;
    public Sprite extractedIcon;
    private Inventory _inventory;
    private UIResourceManager _UI;
    
    private void Start()
    {
        _itemSpinner = GetComponentInParent<Animator>();
        _collider = GetComponent<BoxCollider>();
        _inventory = Inventory.Instance;
        _UI = UIResourceManager.Instance;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && _inventory.checkInventory() < 10)
        {
            if (Inventory.Instance)
            {
                Destroy(transform.parent.gameObject);
                _itemSpinner.enabled = false;
                _collider.enabled = false;
                _UI.useNotif("Picked up " + gameObject.name, UIResourceManager.notifType.GAINED);

                if (this.CompareTag("Weapon"))
                {
                    if (gameObject.name == "Wooden Sword")
                    {
                        Inventory.Instance.AddItem(new Weapon(gameObject.name, ItemType.Weapon, 20, "Sword made of wood, despite the lack of sharp edges, it still provides proper self defense for most adventurers", extractedMesh, extractedIcon, 1, 1.0f, WeaponType.Sword));
                        if (PlayerStats.Instance.CheckWeapon() == null)
                            PlayerStats.Instance.FirstEquip();
                    }
                }
                else if (this.CompareTag("Tool"))
                {
                    if (gameObject.name == "Wooden Axe")
                    {
                        Inventory.Instance.AddItem(new Tool(gameObject.name, ItemType.Tool, 5, "One of the first tools made to ensure proper tree chopping, to think wood can cut wood would be a crime of sort...", extractedMesh, extractedIcon, 1.0f, 1.0f, HarvestType.Wood));
                        if (PlayerStats.Instance.CheckWood() == null)
                            PlayerStats.Instance.FirstEquip();
                    }
                    if (gameObject.name == "Wooden Pickaxe")
                    {
                        Inventory.Instance.AddItem(new Tool(gameObject.name, ItemType.Tool, 5, "Wooden pickaxe to break stone, people need to give more effort to get anything from stone with this", extractedMesh, extractedIcon, 1.0f, 1.0f, HarvestType.Stone));
                        if (PlayerStats.Instance.CheckStone() == null)
                            PlayerStats.Instance.FirstEquip();
                    }
                }
                else if (this.CompareTag("Consumable"))
                {
                    if (gameObject.name == "Green Crystal")
                    {
                        Inventory.Instance.AddItem(new Consumable(gameObject.name, ItemType.Consumable, 1, "A crystal found on strange creatures that are not of our world, its touch, however, soothes the mind and soul, does that mean I have both?", extractedMesh, extractedIcon, ConsumableType.Heal));
                        if (PlayerStats.Instance.CheckCrystal() == null)
                            PlayerStats.Instance.FirstEquip();
                    }
                }
                // itemList = Inventory.Instance.GetAllItems();
                // Debug.Log("Contents of the list:");
                // foreach (Item item in itemList)
                // {
                //     Debug.Log(item.Name);
                //     Debug.Log(item.itemID);
                //     // Debug.Log(item.Type);
                // }
            }
        }
        else
        	_UI.useNotif("Inventory is full. Cannot add more items.", UIResourceManager.notifType.WARNING);
    }
}
