using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        StartCoroutine(LoadAsynchronously(2));
        SceneManager.LoadScene(3);
    }
   

    IEnumerator LoadAsynchronously(int LoadScreen)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadScreen);
        while (operation.isDone == false)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}
