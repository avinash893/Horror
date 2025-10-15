using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public Transform[] Spawners;
    public GameObject[] zombies;
    public int zombieSpawnAmt = 3;

    private float respawnTimer = 10f;
    private float resetTimer = 0f;
    private bool canSpawn = true;

    // Update is called once per frame
    void Update()
    {
        if (canSpawn == false)
        {
            resetTimer += 1 * Time.deltaTime;
            if (resetTimer > respawnTimer)
            {
                canSpawn = true;
                resetTimer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if(other.CompareTag("Player")&& canSpawn == true && SaveScript.zombiesInGameAmt<80)
        {
            for(int i = 0; i < zombieSpawnAmt; i++)
            {
                Instantiate(zombies[Random.Range(0, zombies.Length)],
              Spawners[Random.Range(0, Spawners.Length)].position,
              Spawners[Random.Range(0, Spawners.Length)].rotation);

                SaveScript.zombiesInGameAmt++;
 
            }
            canSpawn = false;
        }
    }
}
