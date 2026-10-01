using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("Target & Camera")]
    public Transform player;
    public Camera minimapCamera;
    public float height = 120f;
    public float orthographicSize = 65f;

    [Header("UI Elements")]
    public RectTransform northIndicator;

    [Header("Settings")]
    public bool rotateWithPlayer = true;

    void Start()
    {
        if (player == null)
        {
            var pGo = GameObject.Find("FPSController");
            if (pGo != null) player = pGo.transform;
        }

        if (minimapCamera != null)
        {
            minimapCamera.orthographic = true;
            minimapCamera.orthographicSize = orthographicSize;
        }

        // Ensure layer 10 (Minimap) is culled from main player view
        var mainCam = GameObject.Find("FirstPersonCharacter");
        if (mainCam != null)
        {
            var cam = mainCam.GetComponent<Camera>();
            if (cam != null)
            {
                cam.cullingMask &= ~(1 << 10);
            }
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Follow player position at overhead altitude
        Vector3 newPos = player.position;
        newPos.y = height;
        transform.position = newPos;

        // Rotate camera with player's Y view angle
        if (rotateWithPlayer)
        {
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

            // Rotate North indicator on HUD to keep true North accurate
            if (northIndicator != null)
            {
                northIndicator.localEulerAngles = new Vector3(0f, 0f, player.eulerAngles.y);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            if (northIndicator != null)
            {
                northIndicator.localEulerAngles = Vector3.zero;
            }
        }
    }
}
