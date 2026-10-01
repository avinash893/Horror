using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChainBlockade : MonoBehaviour
{
    [Header("Dialogue / Caption")]
    public string speakerName = "[SURVIVOR]";
    [TextArea(2, 4)]
    public string dialogueText = "They must be hiding something... I need to break their chain to get out of it.";
    public AudioClip dialogueVoice;

    [Header("Blockade Settings")]
    public bool hasTriggered = false;
    public Animator[] chainedZombieAnimators;
    public AudioSource groanAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (DaySurvivalManager.Instance != null)
            {
                DaySurvivalManager.Instance.PlayRadioCaption(speakerName, dialogueText, dialogueVoice);
            }

            if (groanAudioSource != null)
            {
                groanAudioSource.Play();
            }

            if (chainedZombieAnimators != null)
            {
                foreach (var anim in chainedZombieAnimators)
                {
                    if (anim != null)
                    {
                        anim.SetTrigger("Alert");
                    }
                }
            }
        }
    }
}
