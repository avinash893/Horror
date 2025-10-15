using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombbieAttack : MonoBehaviour
{

    private bool canDamage = false;
    private Collider col;
    private Animator bloodEffect;
    public int damageAmount = 3;
    private AudioSource hitsound;
    // Start is called before the first frame update
    void Start()
    {
        col=GetComponent<Collider>();
        bloodEffect = GameObject.Find("BloodScreen").GetComponent<Animator>();   
        hitsound=GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled == false)
        {
            canDamage = true;
        }


    
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canDamage)
            {
                canDamage = false;

         
                if (SaveScript.health > 0)
                {
                    SaveScript.health -= damageAmount;
                 
                    SaveScript.health = Mathf.Max(0, SaveScript.health);
                }
             
                if (SaveScript.infection < 100)
                {
                    SaveScript.infection += damageAmount;

                    SaveScript.infection = Mathf.Min(100, SaveScript.infection);
                }
                bloodEffect.SetTrigger("Blood");
                hitsound.Play();
            }
        }
    }
}
