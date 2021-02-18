using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimUtil : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    public static CameraAimUtil Instance { get; private set; }

    private Camera cam;

    public Vector3 CurrentPoint
    {
        get
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward * 100f, out hit, 500f, layer))
            {
                if (hit.collider != null)
                    return hit.point;
                
                else {
                    return cam.transform.forward * 500f;
                }
            }
            
            return cam.transform.forward * 500f;
        }
    }

    private void Awake() {
        Instance = this;
        cam = Camera.main;
    }
}
