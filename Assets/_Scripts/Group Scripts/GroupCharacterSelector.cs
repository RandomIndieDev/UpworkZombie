using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;

public class GroupCharacterSelector : MonoBehaviour
{
    public void ShowCharacterDetails(GroupPosition charPosition)
    {
        if (charPosition == null) return;
        if (charPosition.character == null) return;
        
        var character = charPosition.character.GetComponent<BaseCharacter>();
        if (character == null) return;
        
        AudioManager.instance.Play("Open_Char_Data");
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        UIEvents.Instance.DoShowCharacterStats(character.characterStats);
        UIEvents.Instance.DoRenderCharacter(character.transform.GetChild(0).gameObject);
    }
    
}
