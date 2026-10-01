using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightVision : MonoBehaviour
{
    public Image ZoomBar;
    private Image Batterychunk; 
    public Camera cam;
    public float batteryPower = 1.0f;
    public float DrainTime = 2.0f;
    void Start()
    {
        ZoomBar = GameObject.Find("ZoomBar").GetComponent<Image>();
        Batterychunk = GameObject.Find("BatteryInner").GetComponent<Image>();
        cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
       

    }

    private void OnEnable()
    {
        InvokeRepeating("BatteryDrain", DrainTime, DrainTime);

        if (ZoomBar != null)
        {
            ZoomBar.fillAmount = 0.6f;
        }
       
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0)
        {
            if (cam.fieldOfView > 10)
            {
                cam.fieldOfView -= 5;

                ZoomBar.fillAmount = cam.fieldOfView / 100;
            }
        }
        else if (scrollInput < 0)
        {
            if (cam.fieldOfView < 60)
            {
                if (ZoomBar != null && cam != null)
                {
                    ZoomBar.fillAmount = cam.fieldOfView / 100;
                }
            }
        }
     
        if (Batterychunk != null)
        {
            Batterychunk.fillAmount = batteryPower;
        }

       
    }


    private void BatteryDrain()
    {
        if (batteryPower > 0.0f )
        {
            batteryPower = Mathf.Clamp(batteryPower, 0.0f, 1.0f);
            batteryPower -= 0.25f;
            UpdateBatteryUI();
        }
    }

    private void UpdateBatteryUI()
    {
        if (Batterychunk != null)
        {
            Batterychunk.fillAmount = batteryPower;
        }
    }  
    public void stopDrain()
    {
        CancelInvoke("BatteryDrain");
    }
}
