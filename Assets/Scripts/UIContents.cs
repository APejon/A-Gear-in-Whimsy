using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIContents : MonoBehaviour
{
    private int start = 1;
    public GameObject[] Selectors;

    void Start()
    {
        Selectors[0].SetActive(false);
        Selectors[1].SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            start++;
            if (start > 1)
                start = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            start--;
            if (start < 0)
                start = 1;
        }
        if (start == 1)
        {
            Selectors[0].SetActive(true);
            Selectors[1].SetActive(false);
        }
        else if (start == 0)
        {
            Selectors[0].SetActive(false);
            Selectors[1].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (start == 1)
                SceneManager.LoadScene("GearInWhimsy");
            else if (start == 0)
                EditorApplication.isPlaying = false;
        }
    }
}
