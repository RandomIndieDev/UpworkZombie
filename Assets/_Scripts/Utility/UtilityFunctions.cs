using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctions
{
    public static Quaternion GetRotationValueTo(Transform targetTransform, Transform selfTransform)
    {
        Vector3 lookVector = targetTransform.transform.position - selfTransform.position;
        lookVector.y = selfTransform.position.y;
        return Quaternion.LookRotation(lookVector);
    }
}
