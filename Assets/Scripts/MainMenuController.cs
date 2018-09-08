using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    Text loadingText;
    int loadTextIncrement;
    bool loadingInProgress;

    private void Start()
    {
        loadingText = GameObject.Find("LoadingText").GetComponent<Text>();
        loadTextIncrement = 0;
        loadingInProgress = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !loadingInProgress)
        {
            StartCoroutine(LoadMainLevel());
        }
    }

    IEnumerator LoadMainLevel()
    {
        loadingInProgress = true;
        UpdateLoadingText();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        while (!asyncLoad.isDone)
        {
            UpdateLoadingText();
            yield return new WaitForSeconds(1);
        }
    }

    void UpdateLoadingText()
    {
        loadTextIncrement = (loadTextIncrement + 1) % 4;
        string text = "Loading";
        for(int i = 0; i<loadTextIncrement; i++)
        {
            text += ".";
        }
        loadingText.text = text;
    }



}
