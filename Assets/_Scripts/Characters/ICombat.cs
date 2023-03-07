using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat
{
    Transform GetCurrentTransform();
    bool ReceiveDamage(float damage, Vector3 hitOrigin);

    bool IsDead();

    void PrintDetails();

}
