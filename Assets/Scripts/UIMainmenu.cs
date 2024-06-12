using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainmenu : MonoBehaviour
{
    public GameObject skyCamera;
    public GameObject[] windows;
    public float creditDuration = 3f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            windows[i].SetActive(false);
            windows[i].GetComponent<CanvasGroup>().alpha = 0;
        }
        skyCamera.GetComponent<Animator>().enabled = false;
        StartCoroutine(PlayCredits(windows));
    }

    IEnumerator PlayCredits(GameObject[] window)
    {
        for (int i = 0; i < 2; i++)
        {
            window[i].SetActive(true);
            float elapsed = 0f;

            window[i].GetComponent<CanvasGroup>().LeanAlpha(1f, 1);
            while (elapsed < creditDuration)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                    break;
                elapsed += Time.deltaTime;
                yield return null;
            }
            window[i].GetComponent<CanvasGroup>().LeanAlpha(0f, 1);
            yield return new WaitForSeconds(1f);
            window[i].SetActive(false);
        }
        windows[4].GetComponent<CanvasGroup>().LeanAlpha(0.7f, 1);
        skyCamera.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1f);
        windows[2].SetActive(true);
        windows[2].GetComponent<CanvasGroup>().LeanAlpha(1f, 1);
        yield return new WaitForSeconds(2f);
        windows[3].GetComponent<CanvasGroup>().alpha = 1;
        windows[3].SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
