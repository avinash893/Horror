using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HideCube"))
        {


            SaveScript.isHidden = true;

            Debug.Log("yopou are hiding ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideCube"))
        {


            SaveScript.isHidden = false;
        }
    }
}
