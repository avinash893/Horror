using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingScript : MonoBehaviour
{
    private AudioSource audioPlayer;
    private bool heavyBreathing;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.stamina < 20 && !heavyBreathing)
        {
            heavyBreathing = true;
            if (!audioPlayer.isPlaying) 
            {
                audioPlayer.Play();
            }
        }

        if (SaveScript.stamina > 19 && heavyBreathing)
        {
            heavyBreathing = false;
            if (audioPlayer.isPlaying) 
            {
                audioPlayer.Stop();
            }
        }
    }
}
