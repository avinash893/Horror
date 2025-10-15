using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainGui : MonoBehaviour
{
    public Text healthAmt;
    public Text staminaAmt;
    public Text infectionAmt;
    public Color highInfectionColor = Color.red;
    
    public Color normalColor = Color.green;
   


    // Update is called once per frame
    void Update()
    {
        healthAmt.text=SaveScript.health.ToString("F0") +"%";
        staminaAmt.text=SaveScript.stamina.ToString("F0") +"%";
        infectionAmt.text = SaveScript.infection.ToString("F0") + "%";

        if (SaveScript.infection < 20)
        {
            infectionAmt.color = Color.green;
        }
        
       else if ( SaveScript.infection < 80 )
        {
            infectionAmt.color = Color.yellow;
        }
       
        else
        {
            infectionAmt.color = highInfectionColor;
            
        }
    }
}
