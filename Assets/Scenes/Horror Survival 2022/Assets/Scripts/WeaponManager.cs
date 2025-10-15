using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    public enum  weaponSelect
    {
        knife,
        cleaver,
        bat,
        Axe,
        pistol,
        shotgun,
        sprayCan,
        bottle,
        bottleWithCloth
    }

    public weaponSelect choosenWeapon;
    public GameObject[] weapons;
    //private int WeaponId = 0;// Start is called before the first frame update
    private Animator anim;
    private AudioSource audioPlayer;
    public AudioClip[] weaponSounds;
    private int currentWeaponId;

    [Header("Spray Settings")]
    private bool spraySoundOn=false;
    public GameObject sprayPanel;

    private bool sprayEmpty=false;
    private bool stopSpray = false;

   
    public static bool bottleThrow=false;
    public static bool molotovBottleThrow=false;


    public AudioClip[] reloadsounds;

    void Start()
    {
        SaveScript.Weaponid = (int)choosenWeapon;
        
        if (SaveScript.Weaponid < 0 || SaveScript.Weaponid >= weapons.Length)
        {
            SaveScript.Weaponid = (int)WeaponManager.weaponSelect.knife;
        }
        anim = GetComponent<Animator>();
        ChangeWeapons();
        audioPlayer = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    void Update()
    {

        if (SaveScript.isMenuActive == false)
        {
            if (SaveScript.Weaponid != currentWeaponId)
            {
                ChangeWeapons();
            }


            if (SaveScript.inventoryOpen)
            {
                // Reset any queued attacks or input
                anim.ResetTrigger("Attack");
                anim.ResetTrigger("release");
                return; // Skip the attack logic if inventory is open
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (SaveScript.inventoryOpen == false)
                {
                    if (SaveScript.currAmmo[SaveScript.Weaponid] > 0 && SaveScript.stamina > 20)
                    {
                        anim.SetTrigger("Attack");
                        audioPlayer.clip = weaponSounds[SaveScript.Weaponid];
                        audioPlayer.Play();
                        if (SaveScript.Weaponid == 4 || SaveScript.Weaponid == 5)
                        {
                            SaveScript.gunUsed = true;
                            SaveScript.currAmmo[SaveScript.Weaponid]--;
                        }
                    }
                    else
                    {
                        if (SaveScript.Weaponid == 4 || SaveScript.Weaponid == 5)
                        {
                            audioPlayer.clip = weaponSounds[8];
                            audioPlayer.Play();
                        }
                    }
                }
            }

            //for Spray
            if (Input.GetMouseButton(0) && sprayPanel.GetComponent<SprayScript>().sprayAmount > 0.0f)
            {
                sprayEmpty = false;
                stopSpray = false;
                if (SaveScript.Weaponid == (int)weaponSelect.sprayCan && SaveScript.inventoryOpen == false)
                {
                    if (!spraySoundOn)
                    {
                        spraySoundOn = true;
                        anim.SetTrigger("Attack");
                        StartCoroutine(startSpraySound());
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) || sprayPanel.GetComponent<SprayScript>().sprayAmount <= 0.0f)
            {
                if (SaveScript.Weaponid == (int)weaponSelect.sprayCan && !SaveScript.inventoryOpen && stopSpray == false)
                {
                    anim.SetTrigger("release");
                    spraySoundOn = false;
                    audioPlayer.Stop();
                    audioPlayer.loop = false;
                    stopSpray = true;
                }
            }
            if (sprayPanel.GetComponent<SprayScript>().sprayAmount <= 0 && sprayEmpty == false)
            {
                sprayEmpty = true;
                if (SaveScript.weaponAmount[6] > 0)
                    SaveScript.weaponAmount[6]--;
                if (SaveScript.weaponAmount[6] == 0)
                {
                    SaveScript.weaponsPickedUp[6] = false;
                }
            }

            if (SaveScript.Weaponid == 4 || SaveScript.Weaponid == 5)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (SaveScript.currAmmo[SaveScript.Weaponid - 4] > 0)
                    {


                        SaveScript.currAmmo[SaveScript.Weaponid] += SaveScript.ammoAmount[SaveScript.Weaponid - 4];
                        SaveScript.ammoAmount[SaveScript.Weaponid - 4] = 0;


                        anim.SetTrigger("Reload");


                        audioPlayer.clip = reloadsounds[SaveScript.Weaponid - 4];
                        audioPlayer.Play();
                    }
                }

            }
        }
    }
   
   private void ChangeWeapons()
    {
        foreach (GameObject weapon in weapons)
        { 
            weapon.SetActive(false);
        }

        weapons[SaveScript.Weaponid].SetActive(true);
        Debug.Log("Activated weapon: " + SaveScript.Weaponid + " (" + weapons[SaveScript.Weaponid].name + ")");

        choosenWeapon = (weaponSelect)SaveScript.Weaponid;
        anim.SetInteger("weaponId", SaveScript.Weaponid);
        anim.SetBool("weaponChange", true);
        currentWeaponId = SaveScript.Weaponid;
        anim.ResetTrigger("Attack");  // Reset any attack triggers to prevent automatic action
        anim.ResetTrigger("release"); // Reset any release triggers if needed

        MoveWeapon();
        StartCoroutine(WeaponReset());
    }
    private void MoveWeapon()
    {
        Transform weaponTransform = weapons[SaveScript.Weaponid].transform;

        switch (choosenWeapon)
        {
            case weaponSelect.knife:
            case weaponSelect.cleaver:
            case weaponSelect.bat:
                break;
            case weaponSelect.pistol:
                weaponTransform.localPosition = new Vector3(0.0564f, 0.0102f, -0.1063f);
                break;
            case weaponSelect.shotgun:
                break;
            case weaponSelect.Axe:
            
                weaponTransform.localPosition = new Vector3(0.082f, 0.098f, -0.114f);
                break;
            case weaponSelect.sprayCan:
                weaponTransform.localPosition = new Vector3(0.011f, -0.026f, -0.077f);


                break;
            case weaponSelect.bottle:
                weaponTransform.localPosition = new Vector3(-0.047f, -0.139f, -0.059f);


                break;

            default:
             
                break;
        }
    }
    


    public void BottleThrow()
    {
        bottleThrow = true;
    }

    public void MolotovBottleThrow()
    {
        molotovBottleThrow = true;
    }
    public void loadAnotherBottle()
    {
        if (SaveScript.Weaponid == 7 )
        {
            ChangeWeapons();
            

        }

    }

    public void loadAnotherFireBottle()
    {
        if (SaveScript.Weaponid == 8)
        {
            ChangeWeapons();


        }

    }


    IEnumerator WeaponReset()
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("weaponChange", false);
    }

    IEnumerator startSpraySound()
    {
        yield return new WaitForSeconds(0.3f);
        if (spraySoundOn)
        {
            audioPlayer.clip = weaponSounds[SaveScript.Weaponid];
            audioPlayer.Play();
            audioPlayer.loop = true;
        }
    }
}
