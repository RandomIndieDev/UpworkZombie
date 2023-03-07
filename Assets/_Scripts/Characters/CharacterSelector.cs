using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterSelector : MonoBehaviour
{
    public List<GameObject> characters;
    public MeleeWeaponCreator meleeWeaponCreator;        
    public RangedWeaponCreator rangedWeaponCreator;

    private GameObject selectedWeapon;

    private Renderer m_ActiveRenderer;

    public Renderer ActiveRenderer
    {
        get => m_ActiveRenderer;
        set => m_ActiveRenderer = value;
    }

    public void Start()
    {
        SetAllNotActive();

        var index = Random.Range(0, characters.Count);
        characters[index].SetActive(true);
        m_ActiveRenderer = characters[index].GetComponent<Renderer>();
    }

    public GameObject GetRangedWeaponShootPoint()
    {
        return selectedWeapon.transform.Find("ShootLoc").gameObject;
    }

    public void SetupWeapon(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Melee:
                meleeWeaponCreator.RandomizeWeapon();
                break;
            case CharacterType.Sniper:
                selectedWeapon = rangedWeaponCreator.RandomizeWeaponSniper();
                break;
            case CharacterType.Handgun:
                selectedWeapon = rangedWeaponCreator.RandomizeHandgun();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private void SetAllNotActive()
    {
        foreach (var character in characters)
        {
            character.SetActive(false);
        }
    }

}
