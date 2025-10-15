using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamages : MonoBehaviour
{
    private bool damaging=true;
    private int zombieHeath=100;
    private Animator zombieAnim;
    private Collider zombieCollider;
    private AudioSource damageZombie;


    
    public GameObject bloodSplat;
    public string[] weaponTag;

    public int[] damageAmt;
    public AudioClip[] damageSounds;

    private bool death = false;

    private Collider weaponCollider;
    private bool flameDeath = false;

    
    // Start is called before the first frame update
    void Start()
    {
        zombieAnim = GetComponentInParent<Animator>();
        damageZombie=GetComponent<AudioSource>();
        zombieCollider = GetComponent<Collider>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.isMenuActive==false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                damaging = true;
            }
            if (zombieHeath <= 0)
            {
                if (death == false)
                {
                    death = true;
                    zombieAnim.SetTrigger("Dead");
                    zombieAnim.SetBool("IsDead", true);
                }

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
     //   Debug.Log("Trigger entered with: " + other.name); 
        for (int i = 0; i < weaponTag.Length; i++)
        {
            if (other.CompareTag(weaponTag[i]))
            {
                if (damaging == true)
                {
                    damaging = false;
                    zombieHeath -= damageAmt[i];
                    Vector3 pos=other.ClosestPoint(transform.position);
                    Instantiate(bloodSplat,pos,other.transform.rotation);
                    damageZombie.clip=damageSounds[i];
                    this.transform.gameObject.GetComponentInParent<Zombie>().isAngry = true;
                    damageZombie.Play();

                    if (weaponTag[i] == "baseballbat")
                    {
                        zombieAnim.SetTrigger("React");
                    }
                    else if (weaponTag[i]=="axe")
                    {
                        zombieAnim.SetTrigger("AxeReact");
                    }

                    Debug.Log("zombie damage " + zombieHeath);
                }
            }
        }
    }


    public void gunDamage(Vector3 hitPoint)
    {
        
        zombieHeath -= 50;
        if (death == false)
        {
            Instantiate(bloodSplat,hitPoint,this.transform.rotation); 
            death = true;
            zombieAnim.SetTrigger("Dead");
            zombieAnim.SetBool("IsDead", true);
        }
    }

    public void ShotDamage(Vector3 hitPoint)
    {

        zombieHeath -= 100;
        if (death == false)
        {
            Instantiate(bloodSplat, hitPoint, this.transform.rotation);
            death = true;
            zombieAnim.SetTrigger("Dead");
            zombieAnim.SetBool("IsDead", true);
        }
    }

    public void FlameDeath()
    {
        if (flameDeath == false)
        {
            flameDeath = true;
            StartCoroutine(ZombieFireWalk());
        }
    }

    IEnumerator ZombieFireWalk ()
    {
        yield return  new WaitForSeconds(1);
        if (death == false)
        {
            
            death = true;
            zombieAnim.SetTrigger("FireDie");

            yield return new WaitForSeconds(15f);

            zombieAnim.SetBool("IsDead", true);
        }
    }
}
