using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupScript : MonoBehaviour
{
    private RaycastHit hit;

    public LayerMask excludeLayers;

    public GameObject pickupPanel;
    public float pickupDisplayDistance=7f;

    public Image mainImage;
    public Sprite[] weaponSprite;
    public Text mainTitle;
    public string[] WeaponTitle;



    public Sprite[] itemSprite;
 
    public string[] itemTitle;


    public Sprite[] ammoSprite;

    public string[] ammoTitle;

    private int objID = 0;

    private AudioSource audioPlayer;

    public GameObject doorMessageObj;
    public Text doorMessage;
    public AudioClip[] pickupSounds;

    private RaycastHit gunHit;
    private RaycastHit shotHits;
    void Start()
    {
        pickupPanel.SetActive(false);
        audioPlayer = GetComponent<AudioSource>();
        doorMessageObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 20, ~excludeLayers))
        {
            if (Vector3.Distance(transform.position, hit.transform.position) < pickupDisplayDistance)
            {
               




                if (hit.transform.gameObject.CompareTag("weapon"))
                {
                    if (Input.GetKeyUp(KeyCode.T))
                    {
                      
                        pickupPanel.SetActive(true);
                        StartCoroutine(PickupShow());
                      
                    }
                    
                    objID = (int)hit.transform.gameObject.GetComponent<WeaponType>().chooseWeapon;
                    Debug.Log("weapon no is" + objID);
                    mainImage.sprite = weaponSprite[objID];
                    mainTitle.text = WeaponTitle[objID];

                    if (Input.GetKeyUp(KeyCode.E)) 
                    {
                        SaveScript.weaponAmount[objID]++;
                        audioPlayer.clip = pickupSounds[3];

                        audioPlayer.Play();
                        SaveScript.change = true;
                        Destroy(hit.transform.gameObject, 0.25f);

                    }
                }








               else if (hit.transform.gameObject.CompareTag("item"))
                {
                    if (Input.GetKeyUp(KeyCode.T))
                    {

                        pickupPanel.SetActive(true);
                        StartCoroutine(PickupShow());
                       
                    }

                    objID = (int)hit.transform.gameObject.GetComponent<ItemsType>().chooseItem;
                    Debug.Log("item no is" + objID);
                    mainImage.sprite = itemSprite[objID];
                    mainTitle.text = itemTitle[objID];

                      if (Input.GetKeyUp(KeyCode.E))
                 {
                        SaveScript.itemAmount[objID]++;
                        audioPlayer.clip = pickupSounds[3];

                        audioPlayer.Play();
                        SaveScript.change = true;
                        Destroy(hit.transform.gameObject, 0.25f);

                    }
                }


                else if (hit.transform.gameObject.CompareTag("ammo"))
                {
                    if (Input.GetKeyUp(KeyCode.T))
                    {

                        pickupPanel.SetActive(true);
                        StartCoroutine(PickupShow());
                     
                    }

                    objID = (int)hit.transform.gameObject.GetComponent<AmmoType>().chooseAmmo;
                    Debug.Log("item no is" + objID);
                    mainImage.sprite = ammoSprite[objID];
                    mainTitle.text = ammoTitle[objID];

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        if (objID == 0)
                        {
                            SaveScript.ammoAmount[0] += 12;
                        }
                        if (objID == 1)
                        {
                            SaveScript.ammoAmount[1] += 8;
                        }
                        audioPlayer.Play();
                        SaveScript.change = true;
                        Destroy(hit.transform.gameObject, 0.25f);

                    }
                }


                else if (hit.transform.gameObject.CompareTag("door"))
                {
                    SaveScript.doorObj = hit.transform.gameObject;
                   

                    objID = (int)hit.transform.gameObject.GetComponent<DoorType>().chooseDoor;
                    if (hit.transform.gameObject.GetComponent<DoorType>().locked == true) 
                    {
                        doorMessage.text = hit.transform.gameObject.GetComponent<DoorType>().message = "Locked. You need to use the "+ hit.transform.gameObject.GetComponent<DoorType>().chooseDoor + " key";
                    }

                    if (hit.transform.gameObject.GetComponent<DoorType>().locked == false)
                    {
                        doorMessage.text = hit.transform.gameObject.GetComponent<DoorType>().message = "press E to open the door " ;
                    }



                    doorMessageObj.SetActive(true);
                    doorMessage.text=hit.transform.gameObject.GetComponent<DoorType>().message;


                    if (Input.GetKeyUp(KeyCode.E) && hit.transform.gameObject.GetComponent<DoorType>().locked==false)
                    {
                        audioPlayer.clip = pickupSounds[objID];
                        audioPlayer.Play();

                        if (hit.transform.gameObject.GetComponent<DoorType>().opened == false)
                        {
                            hit.transform.gameObject.GetComponent<DoorType>().message = "press E to close the door";
                            hit.transform.gameObject.GetComponent<DoorType>().opened = true;
                            hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Open");
                        }

                        else if (hit.transform.gameObject.GetComponent<DoorType>().opened == true)
                        {
                            hit.transform.gameObject.GetComponent<DoorType>().message = "press E to open the door";
                            hit.transform.gameObject.GetComponent<DoorType>().opened = false;
                            hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Close");
                        }

                        audioPlayer.Play();

                    }
                }


            }
            else
            {
                pickupPanel.SetActive(false);
                doorMessageObj.SetActive(false);
                SaveScript.doorObj=null;
            }
        
         
        
        }


        if (!SaveScript.inventoryOpen && Input.GetMouseButtonDown(0))
        {
            if (SaveScript.Weaponid == 4 && SaveScript.currAmmo[4] > 0)
            {
                if (Physics.SphereCast(transform.position, 0.1f, transform.forward, out gunHit, 500))
                {
                    if (gunHit.transform != null && gunHit.transform.gameObject.name == "Body")
                    {
                        var gd = gunHit.transform.gameObject.GetComponent<GunDamage>();
                        if (gd != null) gd.SendGunDamage(gunHit.point);
                    }
                }
            }
            else if (SaveScript.Weaponid == 5 && SaveScript.currAmmo[5] > 0)
            {
                if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out shotHits, 50))
                {
                    if (shotHits.transform != null && shotHits.transform.gameObject.name == "Body")
                    {
                        var gd = shotHits.transform.gameObject.GetComponent<GunDamage>();
                        if (gd != null) gd.SendShotDamage(shotHits.point);
                    }
                }
            }
        }

    }



    IEnumerator PickupShow()
    {
        yield return new WaitForSeconds(3);
        pickupPanel.SetActive(false); // Disable the panel
    }

}
