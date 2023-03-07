using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributeHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject attributeImageParent;
    public Image attributeImage;
    
    private GameSettingsCharacters gameSettingsCharacters;
    
    public void Init(GameSettingsCharacters settings)
    {
        gameSettingsCharacters = settings;
    }
    public void SetAttributeImage(CharacterAttribute attribute)
    {
        Sprite image = null;
            
        switch (attribute)
        {
            case CharacterAttribute.None:
                return;
            case CharacterAttribute.Medic:
                image = gameSettingsCharacters.medicIcon;
                break;
            case CharacterAttribute.Commander:
                image = gameSettingsCharacters.commanderIcon;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
        }

        attributeImage.sprite = image;
        attributeImageParent.SetActive(true);
    }

    public void DisableTypeImage()
    {
        attributeImageParent.SetActive(false);
    }
}
