using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenLoad : MonoBehaviour
{
    void Update()
    {

        if (Input.anyKeyDown)
        {
            LoadStartScene();
        }
    }

    // Method to load the scene
    public void LoadStartScene()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
        SaveScript.infection = 0;
        SaveScript.health = 100;
        SaveScript.stamina=100; 
    }
}
