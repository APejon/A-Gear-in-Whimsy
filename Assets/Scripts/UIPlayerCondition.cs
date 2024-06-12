using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCondition : MonoBehaviour
{
    private static UIPlayerCondition instance;
    public static UIPlayerCondition Instance {get { return instance; }}
    public Image combat;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (combat)
            combat.enabled = false;
        else
            Debug.LogWarning("Image UI is not assigned!");
    }

    public void ShowCombatImage()
    {
        combat.enabled = true;
    }

    public void HideCombatImage()
    {
        combat.enabled = false;
    }
}
