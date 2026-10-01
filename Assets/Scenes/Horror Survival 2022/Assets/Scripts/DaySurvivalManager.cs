using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DaySurvivalManager : MonoBehaviour
{
    public static DaySurvivalManager Instance;

    [System.Serializable]
    public class DayCheckpoint
    {
        public string checkpointName;
        public Vector3 spawnPosition;
        public float spawnRotationY;
        [TextArea(2, 4)]
        public string radioTransmission;
        public AudioClip voiceClip;
    }

    [Header("Survival Settings")]
    public int currentDay = 1;
    public const int MaxDays = 10;
    public float dayDuration = 180f; // 3 minutes per day
    public float timer;
    public bool isDayActive = true;

    [Header("10 Checkpoint Spawns")]
    public DayCheckpoint[] checkpoints = new DayCheckpoint[10];

    [Header("References")]
    public Transform playerTransform;
    public CharacterController playerCharacterController;
    public YouWonScreen youWonScreen;
    public Transform extractionBoat;
    public float extractionRadius = 8f;
    public AudioSource radioAudioSource;

    [Header("UI - Day & Timer")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timerText;
    public GameObject dayTransitionBanner;
    public TextMeshProUGUI transitionDayText;

    [Header("UI - Captions (Radio / Subtitles)")]
    public GameObject captionPanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI captionBodyText;
    public bool captionsEnabled = true;
    public float captionDuration = 8f;
    public float typewriterSpeed = 0.025f;
    private Coroutine captionCoroutine;
    private bool gameWon = false;

    void Awake()
    {
        Instance = this;
        InitializeCheckpoints();
    }

    void Start()
    {
        if (playerTransform == null)
        {
            var pGo = GameObject.Find("FPSController");
            if (pGo != null)
            {
                playerTransform = pGo.transform;
                playerCharacterController = pGo.GetComponent<CharacterController>();
            }
        }

        if (youWonScreen == null)
        {
            var ywGo = GameObject.Find("YouWon");
            if (ywGo != null) youWonScreen = ywGo.GetComponent<YouWonScreen>();
        }

        if (radioAudioSource == null)
        {
            radioAudioSource = GetComponent<AudioSource>();
            if (radioAudioSource == null) radioAudioSource = gameObject.AddComponent<AudioSource>();
        }

        currentDay = Mathf.Clamp(SaveScript.currentDay, 1, MaxDays);
        SpawnPlayerAtDay(currentDay);
        StartDay(currentDay);
    }

    void Update()
    {
        if (gameWon) return;

        // Toggle caption option with 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            captionsEnabled = !captionsEnabled;
            if (!captionsEnabled && captionPanel != null)
            {
                captionPanel.SetActive(false);
            }
        }

        // Day timer progression
        if (isDayActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateHUD();

                if (timer <= 0)
                {
                    timer = 0;
                    UpdateHUD();
                    OnDayCompleted();
                }
            }
        }

        // Day 10 Extraction Check
        if (currentDay == MaxDays && !gameWon)
        {
            if (extractionBoat != null && playerTransform != null)
            {
                float dist = Vector3.Distance(playerTransform.position, extractionBoat.position);
                if (dist <= extractionRadius)
                {
                    TriggerExtractionWin();
                }
            }
        }
    }

    public void StartDay(int day)
    {
        currentDay = day;
        SaveScript.currentDay = day;
        timer = dayDuration;
        isDayActive = true;
        UpdateHUD();

        int idx = day - 1;
        if (idx >= 0 && idx < checkpoints.Length)
        {
            string transmission = checkpoints[idx].radioTransmission;
            AudioClip voice = checkpoints[idx].voiceClip;
            PlayRadioCaption("[HQ COMMAND RADIO]", transmission, voice);
        }
    }

    public void SpawnPlayerAtDay(int day)
    {
        int idx = Mathf.Clamp(day - 1, 0, checkpoints.Length - 1);
        Vector3 targetPos = checkpoints[idx].spawnPosition;
        float targetRotY = checkpoints[idx].spawnRotationY;

        if (playerTransform != null)
        {
            if (playerCharacterController != null)
            {
                playerCharacterController.enabled = false;
            }

            playerTransform.position = targetPos;
            playerTransform.rotation = Quaternion.Euler(0f, targetRotY, 0f);

            if (playerCharacterController != null)
            {
                playerCharacterController.enabled = true;
            }
        }
    }

    public void OnDayCompleted()
    {
        if (currentDay < MaxDays)
        {
            StartCoroutine(DayTransitionRoutine(currentDay + 1));
        }
        else
        {
            // Reached end of day 10 without getting to boat
            isDayActive = false;
            PlayRadioCaption("[HQ COMMAND RADIO]", "We can't hold the dock much longer! Get to the extraction boat NOW!", null);
        }
    }

    IEnumerator DayTransitionRoutine(int nextDay)
    {
        isDayActive = false;

        if (dayTransitionBanner != null)
        {
            dayTransitionBanner.SetActive(true);
            if (transitionDayText != null)
            {
                transitionDayText.text = "DAY " + currentDay + " SURVIVED\nDAWN HAS ARRIVED";
            }
        }

        yield return new WaitForSeconds(4f);

        if (dayTransitionBanner != null)
        {
            dayTransitionBanner.SetActive(false);
        }

        SpawnPlayerAtDay(nextDay);
        StartDay(nextDay);
    }

    public void TriggerExtractionWin()
    {
        gameWon = true;
        isDayActive = false;
        PlayRadioCaption("[RESCUE TEAM]", "Survivor on board! Extraction successful! We are heading home!", null);

        if (youWonScreen != null)
        {
            youWonScreen.ShowYouWonScreen();
        }
    }

    public void PlayRadioCaption(string speaker, string text, AudioClip voice)
    {
        if (!captionsEnabled) return;

        if (captionCoroutine != null)
        {
            StopCoroutine(captionCoroutine);
        }

        captionCoroutine = StartCoroutine(ShowCaptionRoutine(speaker, text, voice));
    }

    IEnumerator ShowCaptionRoutine(string speaker, string text, AudioClip voice)
    {
        if (captionPanel != null)
        {
            captionPanel.SetActive(true);
        }

        if (speakerText != null)
        {
            speakerText.text = speaker;
        }

        if (voice != null && radioAudioSource != null)
        {
            radioAudioSource.clip = voice;
            radioAudioSource.Play();
        }

        if (captionBodyText != null)
        {
            captionBodyText.text = text;
            captionBodyText.maxVisibleCharacters = 0;
            captionBodyText.ForceMeshUpdate();

            int totalVisible = captionBodyText.textInfo.characterCount;
            for (int i = 0; i <= totalVisible; i++)
            {
                captionBodyText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(typewriterSpeed);
            }
        }

        yield return new WaitForSeconds(captionDuration);

        if (captionPanel != null)
        {
            captionPanel.SetActive(false);
        }
    }

    void UpdateHUD()
    {
        if (dayText != null)
        {
            dayText.text = "DAY " + currentDay + " / " + MaxDays;
        }

        if (timerText != null)
        {
            if (currentDay == MaxDays)
            {
                timerText.text = "OBJECTIVE: REACH BOAT";
            }
            else
            {
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                timerText.text = string.Format("UNTIL DAWN: {0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    void InitializeCheckpoints()
    {
        checkpoints[0] = new DayCheckpoint {
            checkpointName = "Starting Bridge Shelter",
            spawnPosition = new Vector3(455.05f, 28.50f, 148.00f),
            spawnRotationY = 0f,
            radioTransmission = "All survivors, emergency extraction team dispatched! ETA 10 days. Move north and secure shelters along the route!"
        };

        checkpoints[1] = new DayCheckpoint {
            checkpointName = "South-East Forest Cabin",
            spawnPosition = new Vector3(558.00f, 19.10f, 276.00f),
            spawnRotationY = 0f,
            radioTransmission = "Day 2. Heavy infected swarms detected in the valley. Barricade doors behind you and stay low until dawn!"
        };

        checkpoints[2] = new DayCheckpoint {
            checkpointName = "Mid-South Outpost Cabin",
            spawnPosition = new Vector3(500.00f, 22.10f, 363.00f),
            spawnRotationY = 0f,
            radioTransmission = "Day 3. Keep pushing forward! Look for food, water, and ammo caches inside abandoned houses."
        };

        checkpoints[3] = new DayCheckpoint {
            checkpointName = "South-West Brickhouse",
            spawnPosition = new Vector3(453.00f, 20.10f, 434.00f),
            spawnRotationY = 90f,
            radioTransmission = "Day 4. Severe storms delayed helicopter extraction. We've routed an armed evacuation boat to the northern lake!"
        };

        checkpoints[4] = new DayCheckpoint {
            checkpointName = "West Lakeside Cabin 1",
            spawnPosition = new Vector3(418.00f, 18.60f, 478.00f),
            spawnRotationY = 90f,
            radioTransmission = "Day 5. Halfway mark, survivor! Night temperatures are dropping. Use light sparingly to avoid alerting zombies."
        };

        checkpoints[5] = new DayCheckpoint {
            checkpointName = "West Lakeside Cabin 2",
            spawnPosition = new Vector3(406.40f, 18.60f, 532.00f),
            spawnRotationY = 45f,
            radioTransmission = "Day 6. Satellite shows infected gathering near open water. Stay inside fortified buildings after dark!"
        };

        checkpoints[6] = new DayCheckpoint {
            checkpointName = "Central Villa West Wing",
            spawnPosition = new Vector3(484.00f, 21.60f, 508.00f),
            spawnRotationY = 0f,
            radioTransmission = "Day 7. Rescue vessel has entered the outer river channels. Keep moving north towards the lake pier!"
        };

        checkpoints[7] = new DayCheckpoint {
            checkpointName = "Riverfront Villa",
            spawnPosition = new Vector3(555.00f, 18.60f, 455.00f),
            spawnRotationY = 270f,
            radioTransmission = "Day 8. Only two days left until extraction! We are tracking your beacon—hang on!"
        };

        checkpoints[8] = new DayCheckpoint {
            checkpointName = "North-West Hilltop Villa",
            spawnPosition = new Vector3(392.00f, 25.60f, 685.00f),
            spawnRotationY = 90f,
            radioTransmission = "Day 9. Evacuation boat ETA is tomorrow morning at 06:00! Reach the lake extraction perimeter now!"
        };

        checkpoints[9] = new DayCheckpoint {
            checkpointName = "Lake Pier Extraction Dock",
            spawnPosition = new Vector3(628.00f, 18.30f, 710.00f),
            spawnRotationY = 45f,
            radioTransmission = "Day 10. WE HAVE ARRIVED AT THE DOCKS! GET TO THE EXTRACTION BOAT NOW! MOVE!"
        };
    }
}
