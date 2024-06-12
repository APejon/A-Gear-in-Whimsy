using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIGameOver : MonoBehaviour
{
    private int tryAgain = 1;
    public TextMeshProUGUI inspire;
    public GameObject[] Letters;
    public GameObject Cursor;
    public GameObject inspireWButtons;
    public GameObject[] Selectors;
    private string[] inspireArray = new string[] { "Don't give up, you got this!",
                                                    "Get up Whimsy, story's not over!",
                                                    "There is still much to tell, try again...",
                                                    "You can take on more than this, I know it!"};

    public void Start()
    {
        gameObject.SetActive(false);
        GetComponent<CanvasGroup>().alpha = 0;
        inspireWButtons.GetComponentInChildren<CanvasGroup>().alpha = 0;
        for (int i = 0; i < Letters.Length; i++)
            Letters[i].SetActive(false);
        Selectors[0].SetActive(false);
        Selectors[1].SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tryAgain = 1;
            Selectors[0].SetActive(true);
            Selectors[1].SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tryAgain = 0;
            Selectors[0].SetActive(false);
            Selectors[1].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tryAgain == 1)
                RestartButton();
            else if (tryAgain == 0)
                ExitButton();
        }
    }

    public void setup()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0.8f, 1f);
        StartCoroutine(Stutter(2));
        StartCoroutine(TypeIn());
        StartCoroutine(Stutter(3));
    }

    IEnumerator TypeIn()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < Letters.Length; i++)
        {
            Cursor.transform.position += new Vector3(35f, 0f, 0f);
            Letters[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        StartCoroutine(Stutter(3));
        inspire.text = inspireArray[Random.Range(0,inspireArray.Length)];
        inspireWButtons.GetComponentInChildren<CanvasGroup>().LeanAlpha(1f, 1f);
        yield return new WaitForSeconds(2.5f);
    }

    IEnumerator Stutter(int times)
    {
        for (int i = 0; i < times; i++)
        {
            Cursor.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            Cursor.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GearInWhimsy");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
