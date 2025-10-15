using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadBar;
    public Text loadPercent;
    public Text loadingText;
    private bool isAnimating = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
      
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsync(1));
    }


    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true);

        // Start animating the loading text
        StartCoroutine(AnimateLoadingText());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            loadBar.value = Mathf.Lerp(loadBar.value, targetProgress, Time.deltaTime * 5f);
            loadPercent.text = $"{(loadBar.value * 100):0}%";
            yield return null;
        }

        loadBar.value = 1f;
        loadPercent.text = "100%";
        isAnimating = false;

        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
    }


    public void ExitGame()
    {
        // Log a message to indicate the game is exiting
        Debug.Log("Exiting the game...");

        // Exits the application if running outside the editor
        Application.Quit();

        // If running in the Unity Editor, stop playing the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator AnimateLoadingText()
    {
        isAnimating = true;

        string baseText = "Loading";
        int dotCount = 0;

        while (isAnimating)
        {
            dotCount = (dotCount + 1) % 4; 
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f); 
        }
    }
}
