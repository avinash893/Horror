using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public enum TypeOfWeapon
    {
        knife,
        cleaver,
        bat,
        Axe,
        pistol,
        shotgun,
        sprayCan,
        bottle,
        bottleWithCloth
    }

    public TypeOfWeapon chooseWeapon;
}