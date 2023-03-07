using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
        transform.Rotate(0,180,0);
    }
}
