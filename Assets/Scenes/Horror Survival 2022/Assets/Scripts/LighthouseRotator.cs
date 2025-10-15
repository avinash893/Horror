using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseRotator : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    private float RotSpeed = 0.5f;
    

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward*RotSpeed * Time.deltaTime) ;
    }
}
