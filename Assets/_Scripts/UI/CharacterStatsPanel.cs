using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsPanel : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI charName;
    public TextMeshProUGUI levelStat;
    public TextMeshProUGUI expStat;
    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI dmgStat;
    public TextMeshProUGUI typeStat;
    public TextMeshProUGUI atrStat;

    public TextMeshProUGUI expStatHeading;

    private string m_CharacterName;

    public void Start()
    {
        m_CharacterName = "";
    }

    public void ActivatePanel()
    {
        gameObject.SetActive(true);
    }

    public void DeactivatePanel()
    {
        gameObject.SetActive(false);
    }

    public string GetCurrentCharName()
    {
        return m_CharacterName ?? "";
    }

    public void SetupStats(CharacterStats stats)
    {
        var reqExpValue = GameHub.Instance.levelProgressionTracker.GetCurrentRequiredExp(stats.CharLevel);

        m_CharacterName = stats.CharName;
        
        charName.text = stats.CharName;
        levelStat.text = stats.CharLevel.ToString();
        expStatHeading.text = $"EXP<voffset=.1em><size=80%>({reqExpValue})";
        expStat.text = ((int)stats.CharExp).ToString();
        hpStat.text = stats.CharHp.ToString();
        dmgStat.text = stats.CharDmg.ToString();
        dmgStat.text = $"{stats.CharDmg + stats.CharExtraDmg}<voffset=.1em><size=50%>({stats.CharExtraDmg}+)";
        typeStat.text = GetTypeString(stats.CharType);
        atrStat.text = GetAtrString(stats.CharAtr);
    }

    private string GetTypeString(CharacterType type)
    {
        var typeString = "";
        switch (type)
        {
            case CharacterType.Melee:
                typeString = "ME";
                break;
            case CharacterType.Sniper:
                typeString = "SN";
                break;
            case CharacterType.Handgun:
                typeString = "HG";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return typeString;
    }
    
    private string GetAtrString(CharacterAttribute attribute)
    {
        var atrString = "";
        switch (attribute)
        {
            case CharacterAttribute.None:
                atrString = "-";
                break;
            case CharacterAttribute.Medic:
                atrString = "MDC";
                break;
            case CharacterAttribute.Commander:
                atrString = "CMD";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
        }

        return atrString;
    }
}
