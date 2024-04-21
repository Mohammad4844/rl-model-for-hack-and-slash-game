using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float smoothSpeed;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newCameraPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, newCameraPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            target = GameObject.FindWithTag("Player");
        }
    }

    
}
