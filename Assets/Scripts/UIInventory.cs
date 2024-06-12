using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform _canvasRectTransform;
    public RectTransform equippedInventory;
    public RectTransform equippedWeapon;
    public RectTransform equippedWoodTool;
    public RectTransform equippedStoneTool;
    public RectTransform equippedConsumable;
    private Vector2 posInventory;
    private Vector2 posWeapon;
    private Vector2 posWood;
    private Vector2 posStone;
    private Vector2 posConsumable;
    public RectTransform Arrow1;
    public RectTransform Arrow2;
    public RectTransform Arrow3;
    public RectTransform Arrow4;
    private Vector2 Arrow1pos;
    private Vector2 Arrow2pos;
    private Vector2 Arrow3pos;
    private Vector2 Arrow4pos;
    private bool _open;
    
    void Start()
    {
        _canvasRectTransform = canvas.GetComponent<RectTransform>();
        posInventory = equippedInventory.anchoredPosition;
        posWeapon = equippedWeapon.anchoredPosition;
        posWood = equippedWoodTool.anchoredPosition;
        posStone = equippedStoneTool.anchoredPosition;
        posConsumable = equippedConsumable.anchoredPosition;
        Arrow1pos = equippedWeapon.anchoredPosition + new Vector2(65, 0);
        Arrow2pos = equippedWoodTool.anchoredPosition + new Vector2(65, 0);
        Arrow3pos = equippedStoneTool.anchoredPosition + new Vector2(65, 0);
        Arrow4pos = equippedConsumable.anchoredPosition + new Vector2(65, 0);
        _open = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !_open)
        {
            transform.LeanMove(posInventory + new Vector2(140, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedWeapon.transform.LeanMove(posWeapon + new Vector2(25, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedWoodTool.transform.LeanMove(posWood + new Vector2(25, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedStoneTool.transform.LeanMove(posStone + new Vector2(25, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedConsumable.transform.LeanMove(posConsumable + new Vector2(25, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            updateArrows(25);
            _open = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && _open)
        {
            transform.LeanMove(posInventory + new Vector2(0, _canvasRectTransform.rect.height), 0.5f).setEaseInQuint();
            equippedWeapon.transform.LeanMove(posWeapon + new Vector2(0, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedWoodTool.transform.LeanMove(posWood + new Vector2(0, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedStoneTool.transform.LeanMove(posStone + new Vector2(0, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            equippedConsumable.transform.LeanMove(posConsumable + new Vector2(0, _canvasRectTransform.rect.height), 0.5f).setEaseOutQuart();
            updateArrows(-25);
            _open = false;
        }
    }

    public void useArrows(int number)
    {
        if (number == 1)
            Arrow1.transform.LeanMove(Arrow1pos + new Vector2(10, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack().setOnComplete(ReverseMovement);
        else if (number == 2)
            Arrow2.transform.LeanMove(Arrow2pos + new Vector2(10, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack().setOnComplete(ReverseMovement);
        else if (number == 3)
            Arrow3.transform.LeanMove(Arrow3pos + new Vector2(10, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack().setOnComplete(ReverseMovement);
        else if (number == 4)
            Arrow4.transform.LeanMove(Arrow4pos + new Vector2(10, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack().setOnComplete(ReverseMovement);
    }

    void ReverseMovement()
    {
        Arrow1.transform.LeanMove(Arrow1pos + new Vector2(0, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack();
        Arrow2.transform.LeanMove(Arrow2pos + new Vector2(0, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack();
        Arrow3.transform.LeanMove(Arrow3pos + new Vector2(0, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack();
        Arrow4.transform.LeanMove(Arrow4pos + new Vector2(0, _canvasRectTransform.rect.height), 0.2f).setEaseOutBack();
    }

    void updateArrows(int change)
    {
        Arrow1pos = equippedWeapon.anchoredPosition + new Vector2(65 + change, 0);
        Arrow2pos = equippedWoodTool.anchoredPosition + new Vector2(65 + change, 0);
        Arrow3pos = equippedStoneTool.anchoredPosition + new Vector2(65 + change, 0);
        Arrow4pos = equippedConsumable.anchoredPosition + new Vector2(65 + change, 0);
    }
}
