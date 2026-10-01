using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera))]
public class InfectionVisualEffect : MonoBehaviour
{
    public static InfectionVisualEffect Instance;

    [Header("Post Processing Volume")]
    public PostProcessVolume postProcessVolume;

    [Header("Infection Thresholds")]
    [Range(0f, 100f)] public float blurStartThreshold = 90f;
    [Range(0f, 100f)] public float severeBlurThreshold = 96f;

    [Header("Interpolation Speed")]
    public float smoothSpeed = 3.5f;

    private DepthOfField dof;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Vignette vignette;

    // Baseline clear-vision settings
    private const float ClearFocusDistance = 10f;
    private const float ClearAperture = 16f;
    private const float ClearFocalLength = 50f;

    // Current animated values
    private float currentFocusDistance = ClearFocusDistance;
    private float currentAperture = ClearAperture;
    private float currentFocalLength = ClearFocalLength;
    private float currentCA = 0f;
    private float currentVignette = 0.25f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (postProcessVolume == null)
        {
            postProcessVolume = GetComponent<PostProcessVolume>();
            if (postProcessVolume == null)
            {
                postProcessVolume = FindObjectOfType<PostProcessVolume>();
            }
        }

        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGetSettings(out dof);
            postProcessVolume.profile.TryGetSettings(out chromaticAberration);
            postProcessVolume.profile.TryGetSettings(out lensDistortion);
            postProcessVolume.profile.TryGetSettings(out vignette);

            // Ensure baseline settings
            if (dof != null) dof.enabled.value = true;
            if (chromaticAberration != null) chromaticAberration.enabled.value = true;
            if (lensDistortion != null) lensDistortion.enabled.value = true;
        }
    }

    void Update()
    {
        if (postProcessVolume == null) return;

        float infection = Mathf.Clamp(SaveScript.infection, 0f, 100f);

        // Calculate target optical blur & disorientation based on infection
        float targetFocusDistance = ClearFocusDistance;
        float targetAperture = ClearAperture;
        float targetFocalLength = ClearFocalLength;
        float targetCA = 0f;
        float targetVignette = 0.25f;
        float targetDistortion = 0f;

        if (infection > blurStartThreshold)
        {
            // Normalized infection from threshold to 100%
            float t = (infection - blurStartThreshold) / (100f - blurStartThreshold);
            t = Mathf.Clamp01(t);

            // Progressive optical defocus blur:
            // High infection closes the focal plane to inches in front of eyes
            targetFocusDistance = Mathf.Lerp(6f, 0.22f, Mathf.Pow(t, 1.4f));
            targetAperture = Mathf.Lerp(8f, 1.4f, t);
            targetFocalLength = Mathf.Lerp(60f, 140f, t);

            // Toxic chromatic separation (double-vision / optic neuropathy)
            targetCA = Mathf.Lerp(0.08f, 0.75f, t);

            // Sickly tunnel-vision vignette
            targetVignette = Mathf.Lerp(0.28f, 0.58f, t);

            // Feverish breathing / disorientation distortion at high levels
            if (infection > severeBlurThreshold)
            {
                float feverSeverity = (infection - severeBlurThreshold) / (100f - severeBlurThreshold);
                float pulse = Mathf.Sin(Time.time * 2.2f) * (6f * feverSeverity);
                targetDistortion = -pulse;
            }
        }

        // Smoothly interpolate post-processing values to prevent jarring pops
        float dt = Time.deltaTime * smoothSpeed;
        currentFocusDistance = Mathf.Lerp(currentFocusDistance, targetFocusDistance, dt);
        currentAperture = Mathf.Lerp(currentAperture, targetAperture, dt);
        currentFocalLength = Mathf.Lerp(currentFocalLength, targetFocalLength, dt);
        currentCA = Mathf.Lerp(currentCA, targetCA, dt);
        currentVignette = Mathf.Lerp(currentVignette, targetVignette, dt);

        // Apply to post-processing components
        if (dof != null)
        {
            dof.focusDistance.value = currentFocusDistance;
            dof.aperture.value = currentAperture;
            dof.focalLength.value = currentFocalLength;
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = currentCA;
        }

        if (vignette != null)
        {
            vignette.intensity.value = currentVignette;
        }

        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetDistortion, dt);
        }
    }
}
