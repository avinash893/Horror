using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveScript : MonoBehaviour
{
    public static bool inventoryOpen=false;
    public static int Weaponid = 0;
    public static bool[] weaponsPickedUp=new bool[9];
    public static int Itemid = 0;


    public static bool[] itemsPickedUp = new bool[13];
    public static int[] weaponAmount= new int[9];
    public static bool change=false;

    public static int []itemAmount = new int[13];


    public static int[] ammoAmount= new int[2];
    public static int[] currAmmo = new int[9];

    public static float stamina;
    public static float infection;
    public static int health;

    public static GameObject doorObj;
    public static List<GameObject> zommbieChasing = new List<GameObject>();


    public static bool gunUsed=false;



    public static Vector3 bottlePos=new Vector3(0,0,0);
    private bool isShmashed=false;

    public static bool isHidden = false;
    public static int zombiesInGameAmt=0;



    //MainMenu
    public static bool isMenuActive = false;

   

    // Start is called before the first frame update
    void Start()
    {


        stamina = FirstPersonController.FpsStamina;
        

        weaponsPickedUp[0]=true;


        weaponsPickedUp[2]=true;
       
       

        weaponAmount[0] = 1;
       
        itemsPickedUp[1] = true;
        itemAmount[0] = 1;
        itemAmount[1] = 1;

        ammoAmount[0] = 20;
        ammoAmount[1] = 2;
       
      for (int i = 0; i < currAmmo.Length; i++)
        {
            currAmmo[i] = 2;
            
        }
        currAmmo[4] = 12;
        currAmmo[6] = 0;
    }
  

    // Update is called once per frame
    void Update()
    {
        if (FirstPersonController.InventorySwitchOn==true)
        { 
            inventoryOpen = true;
        }
        if (FirstPersonController.InventorySwitchOn == false)
        {
            inventoryOpen = false;
        }

        if(Input.GetAxis("Vertical")!=0 && Input.GetKey(KeyCode.LeftShift) && FirstPersonController.FpsStamina>0.0f)
        {
            if (stamina > 0)
            {
                FirstPersonController.FpsStamina -= 10 * Time.deltaTime;
                stamina = FirstPersonController.FpsStamina;
            }
        }

        if (stamina < 100)
        {
            FirstPersonController.FpsStamina += 3.33f * Time.deltaTime;
            stamina = FirstPersonController.FpsStamina;
        }

        if (Input.GetMouseButtonDown(0) && stamina>10 && Weaponid<4  && inventoryOpen==false)
        {
            FirstPersonController.FpsStamina -= 5;
            stamina = FirstPersonController.FpsStamina;

        }
        if (stamina >= 100)
        {
            FirstPersonController.FpsStamina = stamina;
        }



        if (infection < 50 )
        {
            infection += 0.1f * Time.deltaTime;
        }
        if (infection > 49 && infection<100)
        {
            infection += 0.4f * Time.deltaTime;
            
        }
        if (health <= 0)
        {
            SceneManager.LoadScene(2);
        }






            if (change == true)
        {
            
            change = false;
            for (int i = 1; i < weaponAmount.Length; i++)
            {
                if (weaponAmount[i] > 0)
                {
                    weaponsPickedUp[i] = true;
                }
                else if(weaponAmount[i] <= 0)
                    {
                    weaponsPickedUp[i] = false;
                }
               
            }

            for (int i = 1; i < itemAmount.Length; i++)
            {
                if (itemAmount[i] > 0)
                {
                    itemsPickedUp[i] = true;
                }
                else if (itemAmount[i] <= 0)
                {
                    itemsPickedUp[i] = false;
                }

            }
        }

        if (bottlePos != Vector3.zero) 
        {
            if (isShmashed == false)
            {
                StartCoroutine(ResetBottlePos());
                isShmashed = true;
            }
        }
        IEnumerator ResetBottlePos()
        {
            yield return new WaitForSeconds(20);
            bottlePos = Vector3.zero;
            isShmashed= false;
        }
    }
}
