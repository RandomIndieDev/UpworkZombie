using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributeGenerator : MonoBehaviour
{
    private GameSettingsCharacters gameSettingsCharacters;

    public void Init(GameSettingsCharacters settingsCharacters)
    {
        gameSettingsCharacters = settingsCharacters;
    }

    public CharacterAttribute GetRandomAttribute()
    {
        if (Random.value <= gameSettingsCharacters.attributeAssignChance)
        {
            return Random.value <= .5f ? CharacterAttribute.Commander : CharacterAttribute.Medic;
        }

        return CharacterAttribute.None;
    }
}
