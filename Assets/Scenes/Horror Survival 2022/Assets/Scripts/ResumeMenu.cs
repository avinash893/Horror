using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeMenu : MonoBehaviour
{

    public GameObject Resumemenu;

    // Start is called before the first frame update

    private void Start()
    {
        if (Resumemenu == null)
        {
            Resumemenu = GameObject.Find("Resumemenu");
            if (Resumemenu == null)
            {
                Debug.LogError("Resumemenu GameObject not found. Ensure it is assigned or named correctly.");
            }
        }
        Resumemenu.SetActive(false);
       
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!SaveScript.isMenuActive)
            {
                resumeMenuOn();
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                resumeMenuOff();
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
       
    }


   public void resumeMenuOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Resumemenu.SetActive(true); // Show the menu
        Time.timeScale = 0; // Pause the game
        SaveScript.isMenuActive = true;
        Cursor.lockState = CursorLockMode.None;
    }
   public void resumeMenuOff()
    {
        Resumemenu.SetActive(false); // remove the menu
        Time.timeScale = 1; // Pause the game
        SaveScript.isMenuActive = false;
       
    }

    public void resumeByButton()
    {
        if (SaveScript.isMenuActive == false)
        {
            resumeMenuOn();
            Zombie.backgroundMusicPlayer.Stop();
        }
        if (SaveScript.isMenuActive == true)
        {
            resumeMenuOff();
        }
        

    }
    public void playNew()
    {

        SaveScript.stamina = 100;
        SaveScript.health = 100;
        SaveScript.infection = 0;

        SceneManager.LoadScene(1);


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

}
