using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class modeChange : MonoBehaviour
{
    // Start is called before the first frame update
    private PostProcessVolume vol;
    public PostProcessProfile std;
    public PostProcessProfile NightVision;
    public PostProcessProfile inventory;
    private Light Flashlight;
    [Header("Overlays")]
    public GameObject nightVisionOverlay;
    public GameObject FlOverlay;
    private bool NightSwitch = false;
    private bool FlashLightOnn = false;
    public GameObject InventoryMenu;


    public GameObject combinePannel;

    public GameObject Pointer;





    void Start()
    {
       vol=GetComponent<PostProcessVolume>();
        vol.profile = std;
         Flashlight = GameObject.Find("FlashLight").GetComponent<Light>();
         Flashlight.enabled = false;
        nightVisionOverlay.SetActive(false);
    InventoryMenu.SetActive(false);


}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if (SaveScript.inventoryOpen == false)
                if (NightSwitch == false)
                {
                    {
                        vol.profile = NightVision;
                        nightVisionOverlay.SetActive(true);
                        NightSwitch = true;
                        NigthVisionoff();

                    }
                }
                else if (NightSwitch == true)
                {
                    vol.profile = std;
                    nightVisionOverlay.SetActive(false);
                    nightVisionOverlay.GetComponent<NightVision>().stopDrain();
                    this.gameObject.GetComponent<Camera>().fieldOfView = 60;
                    NightSwitch = false;

                }
        }
        if (NightSwitch == true) 
        {
            NigthVisionoff();
        
        }
        if(FlashLightOnn == true)
        {
            if (FlOverlay.GetComponent<FlScript>().batteryPower <= 0) FlashLifghtOff();
        }



        if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (SaveScript.inventoryOpen == false)
            {


                vol.profile = inventory;
                InventoryMenu.SetActive(true);
                SaveScript.inventoryOpen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Pointer.SetActive(false);


                if (FlashLightOnn == true)
                {
                    FlashLifghtOff();
                    FlOverlay.GetComponent<FlScript>().stopDrain();
                    FlOverlay.SetActive(false);
                    FlashLightOnn = false;
                }
                if (NightSwitch == true)
                {
                    nightVisionOverlay.SetActive(false);
                    nightVisionOverlay.GetComponent<NightVision>().stopDrain();
                    this.gameObject.GetComponent<Camera>().fieldOfView = 60;
                    NightSwitch = false;

                }
            }
            else if (SaveScript.inventoryOpen == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Pointer.SetActive(true);
                vol.profile = std;
                combinePannel.SetActive(false);
                InventoryMenu.SetActive(false);
                SaveScript.inventoryOpen = false;  
            }

        }



        //for flashlight

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (SaveScript.inventoryOpen == false)
            {
                if (FlOverlay == null)
                {
                    Debug.LogError("flashlightOverlay is not assigned in the Inspector.");
                    return;
                }
            }


            if (FlashLightOnn == false) 
            { 
                FlOverlay.SetActive(true);
                Flashlight.enabled = true;
                FlashLightOnn = true;
                if (FlOverlay.GetComponent<FlScript>().batteryPower <= 0) FlashLifghtOff();
            }
            else if(FlashLightOnn == true)
            {
                FlashLifghtOff();
                FlOverlay.GetComponent<FlScript>().stopDrain();
                FlOverlay.SetActive(false);
                FlashLightOnn = false;
            }
        }


        if(NightVision == true)
        {
            NigthVisionoff();
        }
        
    }




    private void NigthVisionoff()
    {
        if (nightVisionOverlay.GetComponent<NightVision>().batteryPower <= 0)
        {
            vol.profile = std;
            nightVisionOverlay.SetActive(false);
            NightSwitch = !NightSwitch;
            this.gameObject.GetComponent<Camera>().fieldOfView = 60;

        }
    }
    private void FlashLifghtOff()
    {
        
        
           
            Flashlight.enabled = false;
            FlOverlay.GetComponent<FlScript>().stopDrain();
            FlashLightOnn =!FlashLightOnn;
            FlOverlay.SetActive(false);
        



    }
}
