using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUiManager : MonoBehaviour
{
    public GameObject pistolPanel, shotGunPanel, sprayPanel;
    public Text pistolTotAmmo;
    public Text shotGunTotAmmo;
    public Text pistolCurAmmo;
    public Text shotGunCurAmmo;
    private bool panelOn = false;
    // Start is called before the first frame update
    void Start()
    {
        pistolPanel.SetActive(false);
        shotGunPanel.SetActive(false);
       sprayPanel.SetActive(false);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.Weaponid == 4)
        {
            if (panelOn == false)
            {
                panelOn = true;
                pistolPanel.SetActive(true);

            }
        }

        if(SaveScript.Weaponid == 5)
        {
            if (panelOn == false)
            {
                panelOn = true;
                
                shotGunPanel.SetActive(true);
            }
        }

        if (SaveScript.Weaponid == 6)
        {
            if (panelOn == false)
            {
                panelOn = true;

                sprayPanel.SetActive(true);
            }
        }

        if (SaveScript.inventoryOpen == true)
        {
            
                
                pistolPanel.SetActive(false);
                shotGunPanel.SetActive(false);
                sprayPanel.SetActive(false);
                panelOn = false;    
            
        }

    }
    private void OnGUI()
    {
        pistolTotAmmo.text = SaveScript.ammoAmount[0].ToString();
        shotGunTotAmmo.text = SaveScript.ammoAmount[1].ToString();
        pistolCurAmmo.text = SaveScript.currAmmo[4].ToString();
        shotGunCurAmmo.text = SaveScript.currAmmo[5].ToString();

    }

}
