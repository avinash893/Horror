using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public virtual void Awake()
    {
        gameObject.layer = 9;
    }
    public abstract void onIntract();
    public abstract void onFocus();
    public abstract void onLoseFocus();


}
