using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterSc : MonoBehaviour
{
    public GameObject LighterObj;
    // Start is called before the first frame update

    void OnEnable()
    {
        LighterObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDisable()
    {    
        LighterObj.SetActive(false);
    }
}
