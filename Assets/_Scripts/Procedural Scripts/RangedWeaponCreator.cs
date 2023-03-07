using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponCreator : MonoBehaviour
{
    public GameObject RandomizeWeaponSniper()
    {
        var weapon = transform.GetChild(0).gameObject;
        weapon.SetActive(true);
        return weapon;
    }

    public GameObject RandomizeHandgun()
    {
        var weapon = transform.GetChild(Random.Range(1, transform.childCount)).gameObject;
        weapon.SetActive(true);
        return weapon;
    }
    
    
}
