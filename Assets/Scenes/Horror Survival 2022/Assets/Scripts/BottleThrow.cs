using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BottleThrow : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public float throwPower = 20f;
    public GameObject bottleObj;
    public GameObject molotovBottleObj;
    public Transform throwpoint;




    LineRenderer line;
    public int linePoint = 75;

    public float startWidth = 0.1f;
    public float endWidth = 0.1f;

    public float pointDistance = 0.03f;

    public LayerMask collideLayer;

    public Material mBlue, mRed;
    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.startWidth = startWidth;
        line.endWidth = endWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.inventoryOpen == false && SaveScript.Weaponid>6)
        {
            float HorizontalRotation = Input.GetAxis("Mouse X") * 2;
            float VericalRotation = Input.GetAxis("Mouse Y") * 2;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalRotation * rotationSpeed, VericalRotation * rotationSpeed));

            if (Input.GetAxis("Mouse Y") > 0)
            {
                if (throwPower < 70)
                {
                    throwPower += 6 * Time.deltaTime;
                }
            }

            if (Input.GetAxis("Mouse Y") < 0)
            {
                if (throwPower > 20)
                {
                    throwPower -= 12 * Time.deltaTime;
                }
            }

            line.positionCount = linePoint;
            List<Vector3> points = new List<Vector3>();

            Vector3 startPos = throwpoint.position;
            Vector3 startVelocity = throwpoint.forward * throwPower;
            if (Input.GetMouseButton(1))
            {
                if(SaveScript.Weaponid==7)  line.material = mBlue; 
                if (SaveScript.Weaponid==8) line.material=mRed;
                for (float i = 0; i < linePoint; i += pointDistance)
                {
                    Vector3 newPoint = startPos + startVelocity * i; // Horizontal movement


                    newPoint.y = startPos.y + startVelocity.y + i + Physics.gravity.y / 2f * i * i;//vertical
                    points.Add(newPoint);

                    if (Physics.OverlapSphere(newPoint, 0.01f, collideLayer).Length > 0)
                    {
                        line.positionCount = points.Count;
                        break;
                    }
                }
                line.SetPositions(points.ToArray());
            }

            if (Input.GetMouseButtonUp(1))
            {
                line.positionCount = 0;
            }

            if (WeaponManager.bottleThrow == true)
            {
                WeaponManager.bottleThrow = false;
                GameObject createBottle = Instantiate(bottleObj, throwpoint.position, throwpoint.rotation);
                createBottle.GetComponentInChildren<Rigidbody>().linearVelocity = throwpoint.transform.forward * throwPower;
                SaveScript.weaponAmount[7]--;
                SaveScript.change = true;

            }

            if (WeaponManager.molotovBottleThrow == true)
            {
                WeaponManager.molotovBottleThrow = false;
                GameObject createBottle = Instantiate(molotovBottleObj, throwpoint.position, throwpoint.rotation);
                createBottle.GetComponentInChildren<Rigidbody>().linearVelocity = throwpoint.transform.forward * throwPower;
                SaveScript.weaponAmount[7]--;
                SaveScript.itemAmount[3]--;
                SaveScript.change = true;

            }
        }
    }
  


}
