using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [System.Serializable]
    public class Mission
    {
        public int id;
        public string title;
        [TextArea(2, 3)]
        public string description;
        public bool isCompleted;
        public bool isActive;
    }

    [Header("Missions")]
    public List<Mission> missions = new List<Mission>();
    public int currentMissionIndex = 0;

    [Header("UI - Mission Overlay (TAB)")]
    public GameObject missionOverlayPanel;
    public TextMeshProUGUI activeMissionHeader;
    public TextMeshProUGUI activeMissionBody;
    public TextMeshProUGUI allMissionsText;

    [Header("UI - Notification Banner")]
    public GameObject bannerObject;
    public TextMeshProUGUI bannerTitleText;
    public TextMeshProUGUI bannerSubtitleText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip chimeClip;

    [HideInInspector]
    public bool isOverlayOpen = false;
    private Coroutine bannerCoroutine;

    void Awake()
    {
        Instance = this;
        InitializeDefaultMissions();
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (chimeClip == null)
        {
            chimeClip = GenerateTacticalChime();
        }

        // Start with Mission 1
        StartCoroutine(StartFirstMissionDelayed());
    }

    IEnumerator StartFirstMissionDelayed()
    {
        yield return new WaitForSeconds(1.5f);
        NotifyNewMission(0);
    }

    void Update()
    {
        // Toggle Mission Overlay with TAB key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMissionOverlay();
        }

        // Close on Escape if open
        if (isOverlayOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMissionOverlay();
        }
    }

    public void ToggleMissionOverlay()
    {
        if (isOverlayOpen)
            CloseMissionOverlay();
        else
            OpenMissionOverlay();
    }

    public void OpenMissionOverlay()
    {
        isOverlayOpen = true;
        if (missionOverlayPanel != null)
        {
            missionOverlayPanel.SetActive(true);
            UpdateOverlayUI();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMissionOverlay()
    {
        isOverlayOpen = false;
        if (missionOverlayPanel != null)
        {
            missionOverlayPanel.SetActive(false);
        }

        if (!SaveScript.inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void NotifyNewMission(int index)
    {
        if (index < 0 || index >= missions.Count) return;

        currentMissionIndex = index;
        for (int i = 0; i < missions.Count; i++)
        {
            missions[i].isActive = (i == index);
        }

        // Play tactical notification chime
        if (audioSource != null && chimeClip != null)
        {
            audioSource.PlayOneShot(chimeClip, 0.9f);
        }

        // Show HUD banner notification
        if (bannerCoroutine != null) StopCoroutine(bannerCoroutine);
        bannerCoroutine = StartCoroutine(ShowBannerRoutine(missions[index]));
    }

    public void CompleteMission(int index)
    {
        if (index >= 0 && index < missions.Count)
        {
            missions[index].isCompleted = true;
            missions[index].isActive = false;

            if (index + 1 < missions.Count)
            {
                NotifyNewMission(index + 1);
            }
        }
    }

    IEnumerator ShowBannerRoutine(Mission mission)
    {
        if (bannerObject != null)
        {
            bannerObject.SetActive(true);

            if (bannerTitleText != null)
                bannerTitleText.text = "NEW MISSION RECEIVED";

            if (bannerSubtitleText != null)
                bannerSubtitleText.text = mission.title + "\n<size=12><color=#94A3B8>[PRESS TAB TO VIEW OBJECTIVES]</color></size>";

            yield return new WaitForSeconds(6f);
            bannerObject.SetActive(false);
        }
    }

    void UpdateOverlayUI()
    {
        if (currentMissionIndex >= 0 && currentMissionIndex < missions.Count)
        {
            var active = missions[currentMissionIndex];
            if (activeMissionHeader != null)
                activeMissionHeader.text = "MISSION " + (currentMissionIndex + 1) + ": " + active.title.ToUpper();

            if (activeMissionBody != null)
                activeMissionBody.text = active.description;
        }

        if (allMissionsText != null)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < missions.Count; i++)
            {
                var m = missions[i];
                string statusTag;
                if (m.isCompleted)
                    statusTag = "<color=#22C55E>[COMPLETED]</color>";
                else if (m.isActive)
                    statusTag = "<color=#38BDF8>[ACTIVE]</color>";
                else
                    statusTag = "<color=#64748B>[LOCKED]</color>";

                sb.AppendLine("<b>" + statusTag + " MISSION " + (i + 1) + ": " + m.title + "</b>");
                sb.AppendLine("<size=13><color=#94A3B8>" + m.description + "</color></size>");
                sb.AppendLine();
            }
            allMissionsText.text = sb.ToString();
        }
    }

    void InitializeDefaultMissions()
    {
        missions.Clear();

        missions.Add(new Mission {
            id = 1,
            title = "Find Safe Spot to Survive Your First Night",
            description = "Explore the bridgehead sector, scavenge weapons from abandoned police cabins, and secure a fortified shelter to withstand the night wave until dawn.",
            isActive = true,
            isCompleted = false
        });

        missions.Add(new Mission {
            id = 2,
            title = "Break the Human Chain and Burn the Factory House",
            description = "A horrific chain of infected guards the main road ahead. Find incendiary materials or explosives to incinerate the factory house and sever their perimeter chain.",
            isActive = false,
            isCompleted = false
        });

        missions.Add(new Mission {
            id = 3,
            title = "Restore the Radio Relay & Reach the Lake Extraction",
            description = "Ascend the communication hill to broadcast your coordinates to the emergency extraction boat before the final evacuation window closes.",
            isActive = false,
            isCompleted = false
        });
    }

    AudioClip GenerateTacticalChime()
    {
        int sampleRate = 44100;
        float duration = 0.85f;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        // Two-tone pleasant tactical alert chime: 784 Hz (G5) -> 1046 Hz (C6)
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float tone1 = Mathf.Sin(2f * Mathf.PI * 784f * t) * Mathf.Exp(-t * 8f);
            float tone2 = 0f;
            if (t > 0.12f)
            {
                float t2 = t - 0.12f;
                tone2 = Mathf.Sin(2f * Mathf.PI * 1046f * t2) * Mathf.Exp(-t2 * 6f);
            }
            samples[i] = Mathf.Clamp((tone1 * 0.4f + tone2 * 0.55f), -1f, 1f);
        }

        AudioClip clip = AudioClip.Create("TacticalMissionChime", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
