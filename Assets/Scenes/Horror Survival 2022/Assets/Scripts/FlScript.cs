using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlScript : MonoBehaviour
{
    private Image FlChunks;
    public float batteryPower = 1.0f;
    public float drainTime = 2f;
    // Start is called before the first frame update
    void OnEnable()
    {
        FlChunks= GameObject.Find("FlChunks").GetComponent<Image>();
        InvokeRepeating("FlBatteryDrain", drainTime, drainTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (FlChunks != null)
        {
            FlChunks.fillAmount = batteryPower;
        }
    }
    private void FlBatteryDrain()
    {
        if (batteryPower > 0.0f)
        {
            batteryPower -= 0.25f;
        }
    }
    
    public void stopDrain()
    {
        CancelInvoke("FlBatteryDrain");
    }

  
}
