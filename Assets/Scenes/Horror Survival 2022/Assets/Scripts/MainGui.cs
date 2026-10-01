using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainGui : MonoBehaviour
{
    [Header("Bar Fills")]
    public Image healthBarFill;
    public Image staminaBarFill;
    public Image infectionBarFill;

    [Header("TextMeshPro Readouts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI infectionText;

    [Header("Legacy UI Text (Fallback)")]
    public Text healthAmt;
    public Text staminaAmt;
    public Text infectionAmt;

    [Header("AAA Restrained Palette")]
    public Color healthNormalColor = new Color(0.78f, 0.16f, 0.16f, 1f);     // Deep arterial crimson
    public Color healthCriticalColor = new Color(0.92f, 0.22f, 0.22f, 1f);   // Bright pulsing warning
    public Color staminaNormalColor = new Color(0.08f, 0.58f, 0.85f, 1f);    // Tactical muted slate-cyan
    public Color staminaLowColor = new Color(0.85f, 0.52f, 0.12f, 1f);       // Exhaustion amber
    public Color infectionCleanColor = new Color(0.12f, 0.65f, 0.32f, 0.8f);  // Low toxicity olive
    public Color infectionWarningColor = new Color(0.82f, 0.48f, 0.08f, 1f); // Moderate toxicity amber
    public Color infectionSevereColor = new Color(0.72f, 0.12f, 0.12f, 1f);  // Lethal septic red

    private float currentHealthFill = 1f;
    private float currentStaminaFill = 1f;
    private float currentInfectionFill = 0f;

    void Update()
    {
        // 1. Target fills (0..1)
        float targetHealth = Mathf.Clamp01(SaveScript.health / 100f);
        float targetStamina = Mathf.Clamp01(SaveScript.stamina / 100f);
        float targetInfection = Mathf.Clamp01(SaveScript.infection / 100f);

        // 2. Smooth physical damping
        currentHealthFill = Mathf.Lerp(currentHealthFill, targetHealth, Time.deltaTime * 7f);
        currentStaminaFill = Mathf.Lerp(currentStaminaFill, targetStamina, Time.deltaTime * 12f);
        currentInfectionFill = Mathf.Lerp(currentInfectionFill, targetInfection, Time.deltaTime * 5f);

        // 3. Health Bar
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealthFill;
            if (targetHealth < 0.25f)
            {
                // Subtle heartbeat pulse
                float pulse = 0.75f + Mathf.PingPong(Time.time * 2.5f, 0.25f);
                healthBarFill.color = new Color(healthCriticalColor.r * pulse, healthCriticalColor.g * pulse, healthCriticalColor.b * pulse, 1f);
            }
            else
            {
                healthBarFill.color = healthNormalColor;
            }
        }

        // 4. Stamina Bar
        if (staminaBarFill != null)
        {
            staminaBarFill.fillAmount = currentStaminaFill;
            staminaBarFill.color = targetStamina < 0.2f ? staminaLowColor : staminaNormalColor;
        }

        // 5. Infection Bar
        if (infectionBarFill != null)
        {
            infectionBarFill.fillAmount = currentInfectionFill;
            if (targetInfection < 0.30f)
                infectionBarFill.color = infectionCleanColor;
            else if (targetInfection < 0.70f)
                infectionBarFill.color = infectionWarningColor;
            else
                infectionBarFill.color = infectionSevereColor;
        }

        // 6. Value Readouts
        if (healthText != null)
        {
            healthText.text = Mathf.CeilToInt(SaveScript.health).ToString();
        }

        if (staminaText != null)
        {
            staminaText.text = Mathf.CeilToInt(SaveScript.stamina).ToString();
        }

        if (infectionText != null)
        {
            if (SaveScript.infection <= 0f)
                infectionText.text = "0%";
            else
                infectionText.text = Mathf.CeilToInt(SaveScript.infection) + "%";
        }

        // 7. Legacy Fallbacks
        if (healthAmt != null) healthAmt.text = SaveScript.health.ToString("F0") + "%";
        if (staminaAmt != null) staminaAmt.text = SaveScript.stamina.ToString("F0") + "%";
        if (infectionAmt != null)
        {
            infectionAmt.text = SaveScript.infection.ToString("F0") + "%";
            if (SaveScript.infection < 20) infectionAmt.color = Color.green;
            else if (SaveScript.infection < 80) infectionAmt.color = Color.yellow;
            else infectionAmt.color = infectionSevereColor;
        }
    }
}
