using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class StandardRPPostProcessingSwitcher : MonoBehaviour
{
    public float transitionDuration = 6f; // Time for full transition
    public float delayBetweenTransitions = 2f; // Delay before switching back
    public PostProcessVolume postProcessVolume; // Single Post Processing Volume
    public PostProcessProfile profile1; // First Profile
    public PostProcessProfile profile2; // Second Profile

    private bool usingProfile1 = true;

    void Start()
    {
        postProcessVolume.profile = profile1; // Start with the first profile
        StartCoroutine(TransitionLoop());
    }

    IEnumerator TransitionLoop()
    {
        while (true)
        {
            // Transition to second profile
            yield return StartCoroutine(TransitionEffect(profile1, profile2));

            // Wait before transitioning back
            yield return new WaitForSeconds(delayBetweenTransitions);

            // Transition back to first profile
            yield return StartCoroutine(TransitionEffect(profile2, profile1));

            // Wait before repeating
            yield return new WaitForSeconds(delayBetweenTransitions);
        }
    }

    IEnumerator TransitionEffect(PostProcessProfile fromProfile, PostProcessProfile toProfile)
    {
        float timer = 0f;

        // Set initial profile
        postProcessVolume.profile = fromProfile;
        postProcessVolume.weight = 1f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / transitionDuration); // Smooth transition

            // Gradually reduce weight of old profile and increase new profile
            postProcessVolume.weight = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // Swap profiles smoothly
        postProcessVolume.profile = toProfile;

        // Smoothly fade back in
        timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / transitionDuration);

            postProcessVolume.weight = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }
    }
}
