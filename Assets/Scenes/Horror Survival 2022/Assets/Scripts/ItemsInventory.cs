using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ItemsInventory : MonoBehaviour
{

    public Sprite[] iconOverview;
    public Image bigIcon;
    public string[] titles;
    public Text title;
    public string[] discriptions;
    public Text discription;
    private AudioSource audioPlayer;
    public AudioClip click, select;
    private int choosenItemNo;

    public Button[] ItemButtons;
    public GameObject useButton;
    public GameObject combinePanel;
    public Text amtsText;

   private int updateHealth;
    private int updateStamina;
    private int updateInfection;


    private bool addhealth = false;
    private bool addStamina = false;
    private bool reduceInfection = false;


    public GameObject flashlightPannel, nightvisionPannel;
    private bool flRefill = false;
    private bool nvRefill = false;




    // Start is called before the first frame update
    void Start()
    {
   
        bigIcon.sprite = iconOverview[0];
        title.text = titles[0];
        discription.text = discriptions[0];
        audioPlayer = GetComponent<AudioSource>();
        useButton.SetActive(false);
        combinePanel.SetActive(false);
    }
    private void OnEnable()
    {


        for (int i = 0; i < ItemButtons.Length; i++)
        {
            if (SaveScript.itemsPickedUp[i] == false)
                {
                    ItemButtons[i].image.color = new Color(1, 1, 1, 0.05f);
                    ItemButtons[i].image.raycastTarget = false;
                }

                if (SaveScript.itemsPickedUp[i] == true)
                {
                    ItemButtons[i].image.color = new Color(1, 1, 1, 1);
                    ItemButtons[i].image.raycastTarget = true;
                }


          
            }
        if (SaveScript.itemAmount[choosenItemNo] <= 0)
        {
            ChooseItem(0);
        }
        ChooseItem(choosenItemNo);

    }

    

    public void fillFlBattery() {
        flRefill = true;
    }


    public void fillNvBattery()
    {
        nvRefill = true;
    }
    public void ChooseItem(int ItemNo)
    {
       
        bigIcon.sprite = iconOverview[ItemNo];
        title.text = titles[ItemNo];
        discription.text = discriptions[ItemNo];
        if (audioPlayer != null)
        {
            audioPlayer.clip = click;
            audioPlayer.Play();
        }
        choosenItemNo = ItemNo;
        Debug.Log(ItemNo);
        amtsText.text = "amts" + SaveScript.itemAmount[ItemNo];

        if (ItemNo < 2)
        {
            useButton.SetActive(false);
            combinePanel.SetActive(false);
        }

        else{
            useButton.SetActive(true);  
            
        }

        if (ItemNo != 8) {
            flRefill = false;
        }

        else if (ItemNo != 9)
        {
            nvRefill = false;
        }
    }
    public void AssignItem()
    {
        SaveScript.Itemid = choosenItemNo;
        audioPlayer.clip = select;
        audioPlayer.Play();

        //keys dont disapear

        if (choosenItemNo != 10 && choosenItemNo != 11)
        {



            if (SaveScript.itemAmount[choosenItemNo] > 0)
            { SaveScript.itemAmount[choosenItemNo]--; }
            ChooseItem(choosenItemNo);



            if (SaveScript.itemAmount[choosenItemNo] == 0)
            {
                SaveScript.itemsPickedUp[choosenItemNo] = false;
                useButton.SetActive(false);
            }
        }


        if (addhealth == true)
        {
            addhealth=false;
            if (SaveScript.health < 100)
            {
                SaveScript.health += updateHealth;
                SaveScript.health=Mathf.Clamp(SaveScript.health, 0, 100); 
            }

        }



        if (addStamina == true) {
            addStamina=false;
            if (SaveScript.stamina < 100)
            {
                SaveScript.stamina += updateStamina;
                SaveScript.stamina=Mathf.Clamp(SaveScript.stamina, 0, 100);
            }
        }





        if (reduceInfection == true)
        {
            if (SaveScript.infection > 0.0f)
            {
                SaveScript.infection -= updateInfection;
                SaveScript.infection = Mathf.Clamp(SaveScript.infection, 0, 100);
            }
        }


        if (flRefill == true)
        {
            flRefill = false;
            flashlightPannel.GetComponent<FlScript>().batteryPower = 1.0f;
        }

        else if (nvRefill == true)
        {
            nvRefill = false;
        nightvisionPannel.GetComponent<NightVision>().batteryPower = 1.0f;
        }

        if (choosenItemNo == 10) // Key for Door 1
        {
            UnlockDoor(1); 
        }
        else if (choosenItemNo == 11) // Key for Door 2
        {
            UnlockDoor(2);
        }

    }

   

    public void AddStamina(int staminaUpdate)
    {
        updateStamina=staminaUpdate;
        addStamina = true;
    }
    public void AddHealth(int healthUpdate)
    {
        updateHealth=healthUpdate;
        addhealth = true;
    }
    public void ReduceIfection(int infectionUpdate)
    {
        updateInfection=infectionUpdate;
        reduceInfection = true;
    }





    //door n key fun

    private void UnlockDoor(int doorType)
    {
        if (SaveScript.doorObj != null)
        {
            DoorType doorComponent = SaveScript.doorObj.GetComponent<DoorType>();

            if ((int)doorComponent.chooseDoor == doorType && doorComponent.locked)
            {
                doorComponent.locked = false;
                Debug.Log("Door unlocked!");
            }
        }
    }

    }
