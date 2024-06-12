using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceManager : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform _canvasRectTransform;
    private static UIResourceManager instance;
    public static UIResourceManager Instance {get { return instance; }}
    private Inventory _inventory;
    public TextMeshProUGUI weaponDurability;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI woodDurability;
    public TextMeshProUGUI woodName;
    public TextMeshProUGUI stoneDurability;
    public TextMeshProUGUI stoneName;
    public TextMeshProUGUI consumableDurability;
    public TextMeshProUGUI consumableName;
    public TextMeshProUGUI woodCounter;
    public TextMeshProUGUI stoneCounter;
    public TextMeshProUGUI crystalCounter;
    private PlayerStats _playerstats;
    public Image equippedWeapon;
    public Image equippedWoodTool;
    public Image equippedStoneTool;
    public Image equippedConsumable;
    public Sprite empty;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject over;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _canvasRectTransform = canvas.GetComponent<RectTransform>();
        _inventory = Inventory.Instance;
        _playerstats = PlayerStats.Instance;
        woodCounter.text = _inventory.getWood(false).ToString() + "/" + _inventory.getWood(true).ToString() + " WOOD";
        stoneCounter.text = _inventory.getStone(false).ToString() + "/" + _inventory.getStone(true).ToString() + " STONE";
        crystalCounter.text = _inventory.getCrystal(false).ToString() + "/" + _inventory.getCrystal(true).ToString() + " CRYSTAL";
        this.updateUI();
    }
    
    public void updateUI()
    {
        woodCounter.text = _inventory.getWood(false).ToString() + "/" + _inventory.getWood(true).ToString() + " WOOD";
        stoneCounter.text = _inventory.getStone(false).ToString() + "/" + _inventory.getStone(true).ToString() + " STONE";
        crystalCounter.text = _inventory.getCrystal(false).ToString() + "/" + _inventory.getCrystal(true).ToString() + " CRYSTAL";
        for (int i = 0; i < _playerstats.maxHealth; i++)
        {
            if (i < _playerstats.health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    public void updateEquipped()
    {
        if (_playerstats.CheckWeapon() != null)
        {
            equippedWeapon.sprite = _playerstats.CheckWeapon().Icon;
            weaponName.text = _playerstats.CheckWeapon().Name;
            weaponDurability.text = _playerstats.CheckWeapon().Durability.ToString();
        }
        else
        {
            equippedWeapon.sprite = empty;
            weaponName.text = "None";
            weaponDurability.text = "-";
        }
        if (_playerstats.CheckWood() != null)
        {
            equippedWoodTool.sprite = _playerstats.CheckWood().Icon;
            woodName.text = _playerstats.CheckWood().Name;
            woodDurability.text = _playerstats.CheckWood().Durability.ToString();
        }
        else
        {
            equippedWoodTool.sprite = empty;
            woodName.text = "None";
            woodDurability.text = "-";
        }
        if (_playerstats.CheckStone() != null)
        {
            equippedStoneTool.sprite = _playerstats.CheckStone().Icon;
            stoneName.text = _playerstats.CheckStone().Name;
            stoneDurability.text = _playerstats.CheckStone().Durability.ToString();
        }
        else
        {
            equippedStoneTool.sprite = empty;
            stoneName.text = "None";
            stoneDurability.text = "-";
        }
        if (_playerstats.CheckCrystal() != null)
        {
            equippedConsumable.sprite = _playerstats.CheckCrystal().Icon;
            consumableName.text = _playerstats.CheckCrystal().Name;
            consumableDurability.text = _playerstats.CheckCrystal().Durability.ToString();
        }
        else
        {
            equippedConsumable.sprite = empty;
            consumableName.text = "None";
            consumableDurability.text = "-";
        }
    }

    public void useArrows(int number)
    {
        gameObject.GetComponentInChildren<UIInventory>().useArrows(number);
    }

    public void useNotif(string toSend, notifType type)
    {
        gameObject.GetComponentInChildren<UINotifications>().setup(toSend, type);
    }

    public enum notifType
    {
        WARNING,
        INTERUPTTED,
        GAINED,
        LOST
    };

    public void gameOver()
    {
        over.SetActive(true);
        gameObject.GetComponentInChildren<UIGameOver>().setup();
    }
}
