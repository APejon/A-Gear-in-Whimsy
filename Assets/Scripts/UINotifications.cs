using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotifications : MonoBehaviour
{
    public Canvas canvas;
    public GameObject[] bars;
    public Image[] icons;
    public TextMeshProUGUI[] texts;
    public Sprite[] reficons;
    public Color[] refcolors;

    public class Messenger
    {
        public GameObject bar;
        public Vector2 barPos;
        public Image color;
        public Image icon;
        public TextMeshProUGUI message;
        public Boolean available;
    }

    private List<Messenger> listOfMessengers;

    void Start()
    {
        listOfMessengers = new List<Messenger>();
        for (int i = 0; i < 5; i++)
        {
            Messenger messenger = new Messenger
            {
                bar = bars[i],
                barPos = bars[i].GetComponent<RectTransform>().anchoredPosition,
                icon = icons[i],
                message = texts[i],
                available = true
            };

            listOfMessengers.Add(messenger);
        }
        
        for (int i = 0; i < 5; i++)
            listOfMessengers[i].bar.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        
    }

    public void setup(string toSend, UIResourceManager.notifType messageType)
    {
        for (int i = 0; i < 5; i++)
        {
            if (listOfMessengers[i].available)
            {
                listOfMessengers[i].available = false;
                listOfMessengers[i].icon.sprite = reficons[(int)messageType];
                listOfMessengers[i].message.text = toSend;
                listOfMessengers[i].bar.GetComponent<Image>().color = refcolors[(int)messageType];
                StartCoroutine(notify(listOfMessengers[i]));
                break ;
            }
        }
    }

    IEnumerator notify(Messenger index)
    {
        index.bar.GetComponent<CanvasGroup>().LeanAlpha(0.8f, 1f);
        index.bar.GetComponent<RectTransform>().transform.LeanMove(index.barPos + new Vector2(canvas.GetComponent<RectTransform>().rect.width - 340, 0), 0.5f);

        yield return new WaitForSeconds(3.0f);

        index.bar.GetComponent<RectTransform>().transform.LeanMove(index.barPos + new Vector2(canvas.GetComponent<RectTransform>().rect.width, 0), 0.5f);
        index.bar.GetComponent<CanvasGroup>().LeanAlpha(0f, 1f);
        index.available = true;
    }
}
