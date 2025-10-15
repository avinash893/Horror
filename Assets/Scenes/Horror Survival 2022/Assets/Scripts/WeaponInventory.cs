using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class WeaponInventory : MonoBehaviour
{
    public Sprite[] iconOverview;
    public Image bigIcon;
    public string[] titles;
    public Text title;
    public string[] discriptions;
    public Text discription;
    private AudioSource audioPlayer;
    public AudioClip click, select;
    private int choosenWeaponNo;

    public Button[] weaponButtons;
    public GameObject useButtons,combineButton;
    public GameObject CombinePanel, CombineUsebutton;
    public Image[] combineItems;
    public Text amtText;
    public GameObject sprayPannel;




    // Start is called before the first frame update
    void Start()
    {
        bigIcon.sprite = iconOverview[0];
        title.text = titles[0];
        discription.text = discriptions[0];
        amtText.text = "Amts: 1";
        audioPlayer = GetComponent<AudioSource>();
        CombinePanel.SetActive(false);   
        combineButton.SetActive(false);
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (SaveScript.weaponsPickedUp[i] == false)
            {
                weaponButtons[i].image.color = new Color(1, 1, 1, 0.05f);
                weaponButtons[i].image.raycastTarget = false;
            }

            if (SaveScript.weaponsPickedUp[i] == true)
            {
                weaponButtons[i].image.color = new Color(1, 1, 1, 1);
                weaponButtons[i].image.raycastTarget = true;
            }
        }
        if (choosenWeaponNo <6)
        {
            CombinePanel.SetActive(false);
            combineButton.SetActive(false);
        }
        if (SaveScript.weaponAmount[choosenWeaponNo] <= 0)
        {
            ChooseWeapon(0);
        }
        ChooseWeapon(choosenWeaponNo);
    }


    private void UpdateCombineItems()
    {
        // For item 2
        if (SaveScript.itemsPickedUp[2] == true)
        {
            combineItems[0].color = new Color(1, 1, 1, 1);
        }
        else
        {
            combineItems[0].color = new Color(1, 1, 1, 0.1f);
        }

        // For item 3
        if (SaveScript.itemsPickedUp[3] == true)
        {
            combineItems[1].color = new Color(1, 1, 1, 1);
        }
        else
        {
            combineItems[1].color = new Color(1, 1, 1, 0.1f);
        }
    }



    public void ChooseWeapon(int weaponNumber)
    {
       bigIcon.sprite = iconOverview[weaponNumber];
        title.text= titles[weaponNumber];
        discription.text = discriptions[weaponNumber];
        if (audioPlayer != null)
        {
            audioPlayer.clip = click;
            audioPlayer.Play();
        }
        choosenWeaponNo= weaponNumber;
        Debug.Log(weaponNumber);
        amtText.text = "amts" + SaveScript.weaponAmount[weaponNumber];

        if (choosenWeaponNo >5)
        {
            combineButton.SetActive(true);

        }
        if (choosenWeaponNo < 6)
        {
            CombinePanel.SetActive(false);
            combineButton.SetActive(false);
        }


        if (SaveScript.itemsPickedUp[2] == true) 
        {
            combineItems[0].color=new Color(1, 1,1,1);
        
         }
        else if (SaveScript.itemsPickedUp[2] == false)
        {
            combineItems[0].color = new Color(1, 1, 1, 0.06f);

        }

        else if (SaveScript.itemsPickedUp[3] == true)
        {
            combineItems[1].color = new Color(1, 1, 1, 1);

        }
        else if (SaveScript.itemsPickedUp[3] == false)
        {
            combineItems[1].color = new Color(1, 1, 1, 0.0f);

        }
        UpdateCombineItems();

    }

    public void CombineAssignWeapon()
    {
        if (choosenWeaponNo == 6)
        {
            SaveScript.Weaponid = choosenWeaponNo;
            if (sprayPannel.GetComponent<SprayScript>().sprayAmount<=0.0f)
            sprayPannel.GetComponent<SprayScript>().sprayAmount = 1.0f;
        }
        if (choosenWeaponNo == 7)
        {
            SaveScript.Weaponid = choosenWeaponNo+=1;
        }


        audioPlayer.clip = select;
        audioPlayer.Play();

    }
    public void AssignWeapon()
    {
        SaveScript.Weaponid=choosenWeaponNo;
        audioPlayer.clip=select;
        audioPlayer.Play(); 
    }

    public void CombineAction()
    {
        CombinePanel.SetActive(true);

       
        if (choosenWeaponNo == 7 && SaveScript.itemsPickedUp[2] == true && SaveScript.itemsPickedUp[3] == true)
        {
            combineItems[1].transform.gameObject.SetActive(true);
            CombineUsebutton.SetActive(true);
        }
        else if (choosenWeaponNo == 6 && SaveScript.itemsPickedUp[2] == true)
        {
            combineItems[1].transform.gameObject.SetActive(false);
            CombineUsebutton.SetActive(true);
        }
        else
        {
            CombineUsebutton.SetActive(false);
        }

    }

};
