using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSmash : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioplayer;
    private bool playSound = false;
    private Rigidbody rb;
    public GameObject bottleParent;

    public bool flames=false;
    public GameObject explosion;
    public float destroyTime=3f;
    
    void Start()
    {
        audioplayer = GetComponent<AudioSource>();
        rb=GetComponent<Rigidbody>();
        Destroy(bottleParent,20);
       
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bottle collided with object: " + collision.gameObject.name);

        // Check for all contact points and log details about each one
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("Contact point with: " + contact.otherCollider.name +
                      " at position: " + contact.point);
        }


        if (playSound == false) {
            playSound = true;
        
        audioplayer.Play();
        rb.isKinematic=true;
            SaveScript.bottlePos = this.transform.position;
        Destroy(bottleParent,destroyTime);
        }
        if (flames == true)
        {
            Instantiate(explosion,this.transform.position,this.transform.rotation);
        }




    }
}
