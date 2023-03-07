using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponCreator : MonoBehaviour
{
    public void RandomizeWeapon()
    {
        transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
    }
}
