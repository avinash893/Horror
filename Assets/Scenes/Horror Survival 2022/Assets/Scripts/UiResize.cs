using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiResize : MonoBehaviour
{
    public float scalevalue = 1f;
    public float UHDScale = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width >= 1980)
        {
            scalevalue *= UHDScale;
        }

        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(
            originalScale.x * scalevalue,
            originalScale.y * scalevalue,
            originalScale.z * scalevalue
        );
    }
}