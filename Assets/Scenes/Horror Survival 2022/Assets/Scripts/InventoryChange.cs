using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChange : MonoBehaviour
{
    public GameObject weaponPanel, ItemsPanel,combinePanel;
    // Start is called before the first frame update
    void Start()
    {
        weaponPanel.SetActive(true);  
        ItemsPanel.SetActive(false);
        
    }


    public void SwItemsOn()
    {
        weaponPanel.SetActive(false);
        ItemsPanel.SetActive(true);
        combinePanel.SetActive(false);
    }
    public void SwWeaponsOn()
    {
        weaponPanel.SetActive(true );
        ItemsPanel.SetActive(false);
        combinePanel.SetActive(false);
    }
}
