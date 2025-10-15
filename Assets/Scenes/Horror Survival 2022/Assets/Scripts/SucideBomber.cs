using UnityEngine;

public class SucideBomber : MonoBehaviour
{
    private float distanceToPlayer;
    public float attackDistance = 2f;
    private GameObject Player;
    public ParticleSystem blow; // Ensure to assign this in the Inspector
    private bool adding = true;
    public AudioSource blowSound;
    public AudioClip blowClip;

    private bool hasPlayedSound = false; // Flag to track if sound has already played

    // Start is called before the first frame update
    void Start()
    {
        blowSound = gameObject.GetComponent<AudioSource>();
        Player = GameObject.Find("FPSController");

        if (Player == null)
        {
            Debug.LogError("Player not found! Ensure the player object has the 'FPSController' tag.");
        }

        if (blow == null)
        {
            Debug.LogError("Particle system for explosion not assigned! Please assign a ParticleSystem in the Inspector.");
        }

        if (blowSound == null || blowClip == null)
        {
            Debug.LogError("AudioSource or AudioClip is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        // Check if player is within attack range
        if (distanceToPlayer <= attackDistance)
        {
            // Check if sound has not already played
            if (!hasPlayedSound)
            {
                if (blow != null)
                {
                    if (blowSound != null && blowClip != null)
                    {
                        blowSound.clip = blowClip;  // Assign the explosion clip
                        Debug.Log("Playing explosion sound");
                        blowSound.Play(); // Play the explosion sound
                        Instantiate(blow, transform.position, Quaternion.identity); // Instantiate the explosion effect
                    }
                    else
                    {
                        Debug.LogWarning("blowSound or blowClip is not assigned properly.");
                    }
                }

                // Apply damage and infection
                SaveScript.health -= 25;
                SaveScript.infection += 10;

                // Remove from chasing list
                if (SaveScript.zommbieChasing.Contains(gameObject))
                {
                    SaveScript.zommbieChasing.Remove(gameObject);
                    adding = true;
                }

                // Set the flag so the sound doesn't play again
                hasPlayedSound = true;

                // Destroy the object after a delay to let sound play
                Destroy(gameObject, blowSound.clip.length);
            }
        }
    }
}
