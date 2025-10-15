using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamage : MonoBehaviour
{

    public GameObject zombieDamageObj;
    public GameObject flamesObj;
    // Start is called before the first frame update
    public void SendGunDamage(Vector3 hitPoint)
    {
        zombieDamageObj.GetComponent<ZombieDamages>().gunDamage(hitPoint);
    }

    public void SendShotDamage(Vector3 hitPoint)
    {
        zombieDamageObj.GetComponent<ZombieDamages>().ShotDamage(hitPoint);
    }
    private void OnParticleCollision(GameObject other)
    {
        zombieDamageObj.GetComponent<ZombieDamages>().FlameDeath();
        flamesObj.SetActive(true);
    }
}
