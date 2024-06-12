using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    public int itemID;
    public string Name { get; private set; }
    public ItemType Type { get; private set; }
    public int Durability { get; private set; }
    public string Description { get; private set; }
    public GameObject Mesh { get; private set; }
    public Sprite Icon { get; private set;}

    //constructor for class
    public Item(string name, ItemType type, int durability, string description, GameObject mesh, Sprite icon)
    {
        Name = name;
        Type = type;
        Durability = durability;
        Description = description;
        Mesh = mesh;
        Icon = icon;
    }

    public void DurabilityDecrease()
    {
        Durability--;
    }
}

public class Weapon : Item
{
    public int Damage { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public WeaponType Style { get; protected set; }

    public Weapon(string name, ItemType type, int durability, string description, GameObject mesh, Sprite icon, int damage, float attackSpeed, WeaponType style): base(name, type, durability, description, mesh, icon)
    {
        Damage = damage;
        AttackSpeed = attackSpeed;
        Style = style;
    }

}

public class Tool : Item
{
    public float HarvestSpeed { get; protected set; }
    public float Harvestmodifier { get; protected set; }
    public HarvestType Material { get; protected set; }

    public Tool(string name, ItemType type, int durability, string description, GameObject mesh, Sprite icon, float harvestSpeed, float harvestmodifier, HarvestType material): base(name, type, durability, description, mesh, icon)
    {
        HarvestSpeed = harvestSpeed;
        Harvestmodifier = harvestmodifier;
        Material = material;
    }
}

public class Consumable : Item
{
    public ConsumableType Effect { get; protected set; }
    
    public Consumable(string name, ItemType type, int durability, string description, GameObject mesh, Sprite icon, ConsumableType effect): base(name, type, durability, description, mesh, icon)
    {
        Effect = effect;
    }
}

public enum ItemType
{
    Weapon,
    Tool,
    Consumable,
    Armor
};

public enum WeaponType
{
    Sword
};

public enum HarvestType
{
    None,
    Wood,
    Stone
};

public enum ConsumableType
{
    Heal,
    Damage,
    Speed
};