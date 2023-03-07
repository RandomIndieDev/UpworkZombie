using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnCurve : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private Vector3 m_Offset;

    void Update()
    {
        transform.position = followTransform.position + m_Offset;
    }
}
