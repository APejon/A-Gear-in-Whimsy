using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    public static Inventory Instance {get { return instance; }}
    private List<Item> inventoryItems = new List<Item>();
    private int maxCapacity = 10;
    private int woodStored = 0;
    private int woodCapacity = 99;
    private int stoneStored = 0;
    private int stoneCapacity = 99;
    private int crystalStored = 0;
    private int crystalCapacity = 99;
    private UIResourceManager _UI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _UI = UIResourceManager.Instance;
    }

    public int MaxCapacity
    {
        get { return maxCapacity; }
        set
        {
            if (value > maxCapacity)
                maxCapacity = value;
            else if (value <= maxCapacity)
                Debug.LogWarning("Maximum Capacity cannot be reduced");
        }
    }

    public void AddItem(Item item)
    {
        int setID = 0;

        if (inventoryItems.Count < maxCapacity)
        {
            foreach(Item entry in inventoryItems)
            {
                if (setID == entry.itemID)
                {
                    setID++;
                    continue ;
                }
            }
            item.itemID = setID;
            inventoryItems.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);
    }

    public List<Item> GetAllItems()
    {
        return inventoryItems;
    }

    public void AddWood(int amount)
    {
        if (woodStored < woodCapacity)
        {
            if (amount != 0)
                _UI.useNotif("Gained " + amount + " wood", UIResourceManager.notifType.GAINED);
            woodStored += amount;
        }
        else
            Debug.LogWarning("Maximum wood capacity reached!");
        if (woodStored > woodCapacity)
            woodStored = woodCapacity;
    }

    public void RemoveWood(int amount)
    {
        if (amount <= woodStored)
            woodStored -= amount;
        else
            Debug.LogWarning("Insufficient wood");
    }

    public void AddStone(int amount)
    {
        if (stoneStored < stoneCapacity)
        {
            if (amount != 0)
                _UI.useNotif("Gained " + amount + " stone", UIResourceManager.notifType.GAINED);
            stoneStored += amount;
        }
        else
            Debug.LogWarning("Maximum Stone capacity reached!");
        if (stoneStored > stoneCapacity)
            stoneStored = stoneCapacity;
    }

    public void RemoveStone(int amount)
    {
        if (amount <= stoneStored)
            stoneStored -= amount;
        else
            Debug.LogWarning("Insufficient stone");
    }

    public void AddCrystal(int amount)
    {
        if (crystalStored < crystalCapacity)
        {
            if (amount != 0)
                _UI.useNotif("Gained " + amount + " crystal", UIResourceManager.notifType.GAINED);
            crystalStored += amount;
        }
        else
            Debug.LogWarning("Maximum Crystal capacity reached!");
        if (crystalStored > crystalCapacity)
            crystalStored = crystalCapacity;
    }

    public void RemoveCrystal(int amount)
    {
        if (amount <= crystalStored)
            crystalStored -= amount;
        else
            Debug.LogWarning("Insufficient crystal");
    }

    public int getWood(bool max)
    {
        if (!max)
            return (this.woodStored);
        else
            return (this.woodCapacity); 
    }

    public int getStone(bool max)
    {
        if (!max)
        	return (this.stoneStored);
		else
			return (this.stoneCapacity);
    }

    public int getCrystal(bool max)
    {
        if (!max)
            return (this.crystalStored);
        else
            return (this.crystalCapacity);
    }

    public int checkInventory()
    {
        foreach (Item item in inventoryItems)
        {
            if (item.Durability == 0)
            {
                _UI.useNotif(item.Name + " broke...", UIResourceManager.notifType.LOST);
                inventoryItems.Remove(item);
                break ;
            }
        }
        return (inventoryItems.Count);
    }

    public Item findNext(Item original, ItemType type, HarvestType subType)
    {
        int itemID = -1;
        if (original != null)
            itemID = original.itemID;
        for (int start = itemID + 1; start < inventoryItems.Count; start++)
        {
            if (inventoryItems[start].Type == type)
            {
                if (subType != HarvestType.None)
                {
                    Tool temp = (Tool)inventoryItems[start];
                    if (temp.Material == subType)
                        return (inventoryItems[start]);
                    else
                        continue ;
                }
                else
                    return (inventoryItems[start]);
            }
        }
        if (itemID != -1)
        {
            for (int start = 0; start < itemID; start++)
            {
                if (inventoryItems[start].Type == type)
                {
                    if (subType != HarvestType.None)
                    {
                        Tool temp = (Tool)inventoryItems[start];
                        if (temp.Material == subType)
                            return (inventoryItems[start]);
                        else
                            continue ;
                    }
                    else
                        return (inventoryItems[start]);
                }
            }
        }
        return (original);
    }
}
