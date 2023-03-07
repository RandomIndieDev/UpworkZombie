using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsLevelProgression", menuName = "Game Settings/Progression", order = 1)]
public class GameSettingsLevelProgression : ScriptableObject
{
    
    [Header("Character Stat Changes Per Level")]
    public float m_StatDifferencePercentage;
    public float m_StatIncreasePercentage;

    [Header("Character Special Ability Stat Changes")]
    public float m_SpecialAbilityDifferencePercentage;
    public float m_HealerHealPercentageIncrease;
    public float m_CommanderDamageBuffIncrease;


}
