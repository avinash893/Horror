using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{

    public Text EscText; // Assign this in the Inspector to the text you want to blink.
    public float blinkInterval = 0.5f; // Time between blinks, adjustable in the Inspector.

    private bool isBlinking = false;

    void Start()
    {
        if (EscText != null)
        {
            StartCoroutine(BlinkText());
        }
        else
        {
            Debug.LogError("EscText is not assigned in the Inspector.");
        }
    }

    IEnumerator BlinkText()
    {
        isBlinking = true;
        while (isBlinking)
        {
            EscText.enabled = !EscText.enabled; // Toggle text visibility.
            yield return new WaitForSeconds(blinkInterval); // Wait for the blink interval.
        }
    }

}
