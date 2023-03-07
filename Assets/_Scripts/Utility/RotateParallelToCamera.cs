using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParallelToCamera : MonoBehaviour
{
    public GameObject m_MainCamera;

    void Awake()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up,  m_MainCamera.transform.position - transform.position);
    }
}
