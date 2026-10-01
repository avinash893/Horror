using System.Collections; // Required for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class YouWonScreen : MonoBehaviour
{
    public Image img; // ✅ Assign the Image (UI) from Canvas
    public TextMeshProUGUI textToBlink; // Assign your TextMeshPro text here
    public float blinkSpeed = 0.5f; // Adjust speed of blinking

    private bool isBlinking = false;

    void Start()
    {
        if (!isBlinking && img != null)
        {
            img.gameObject.SetActive(false); // Ensure UI is hidden at start
        }
    }

    void Update()
    {
        if (isBlinking && Input.anyKeyDown) // ✅ Only check input when blinking is active
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SaveScript.currentDay = 1;
            SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual scene name
        }
    }

    // ✅ Public function to activate UI and start blinking
    public void ShowYouWonScreen()
    {
        gameObject.SetActive(true);
        if (img != null) img.gameObject.SetActive(true);
        isBlinking = true; // Allow update checks
        StopAllCoroutines();
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            textToBlink.enabled = !textToBlink.enabled; // Toggle text visibility
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
