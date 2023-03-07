using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats
{
    private string charName;
    private int charLevel;
    private float charExp;
    private int charHP;
    private int charDmg;
    private int charExtraDmg;



    private CharacterType charType;
    private CharacterAttribute charAtr;

    public CharacterStats(string charName, int charLevel, int charExp, int charHp, int charDmg, CharacterType charType, CharacterAttribute charAtr)
    {
        this.charName = charName;
        this.charLevel = charLevel;
        this.charExp = charExp;
        this.charHP = charHp;
        this.charDmg = charDmg;
        this.charType = charType;
        this.charAtr = charAtr;
        this.charExtraDmg = 0;
    }

    public string CharName => charName;

    public CharacterType CharType => charType;

    public CharacterAttribute CharAtr => charAtr;

    public int CharLevel
    {
        get => charLevel;
        set => charLevel = value;
    }

    public float CharExp
    {
        get => charExp;
        set => charExp = value;
    }

    public int CharHp
    {
        get => charHP;
        set => charHP = value;
    }

    public int CharDmg
    {
        get => charDmg;
        set => charDmg = value;
    }
    
    public int CharExtraDmg
    {
        get => charExtraDmg;
        set => charExtraDmg = value;
    }
}

public enum CharacterType
{
    Melee,
    Sniper,
    Handgun
}

public enum CharacterAttribute
{
    None,
    Medic,
    Commander
}
