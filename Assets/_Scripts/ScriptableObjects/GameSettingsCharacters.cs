using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsCharacters", menuName = "Game Settings/Characters", order = 1)]
public class GameSettingsCharacters : ScriptableObject
{

    [Header("Spawnable Survivors")] 
    public GameObject meleeSurvivor;
    public GameObject handgunSurvivor;
    public GameObject sniperSurvivor;

    public float newSurvivorSpawnChance;

    [Header("Attribute Settings")] 
    public Sprite commanderIcon;
    public Sprite medicIcon;

    public float attributeAssignChance;

    public float healAuraTickRate;
    public float baseHealAura;

    public float baseCommanderDamageIncrease;

    [Header("Other Settings")] 
    public Color survivorHitColor;
    public float survivorHitColorDuration;

    [Header("Melee Stats")] 
    public float startingMeleeHealth;
    public float startingMeleeDamage;
    public float meleeAttackRange;

    [Header("HandGun Stats")] 
    public float startingHandgunHealth;
    public float startingHandgunDamage;
    public float startingHandgunRange;
    
    [Header("Sniper Stats")]
    public float startingSniperHealth;
    public float startingSniperDamage;
    public float startingSniperRange;

    [Header("Sniper Projectile Effect")] 
    public float slowDownAmtPercentage;
    public float slowDownTime;
    
    [Header("Health Settings")] 
    public float passiveHealPercentage;
    public float passiveHealTickRate;

    [Header("New Survivor Settings")] 
    public float survivorRejectedAggroChance;
    [Space(20)]
    public float survivorLevel5Chance;
    public float survivorLevel4Chance;
    public float survivorLevel3Chance;
    public float survivorLevel2Chance;
    public float survivorLevel1Chance;
    [Space(20)]
    public float survivor1SpawnChance;
    public float survivor2SpawnChance;
    public float survivor3SpawnChance;

    [Header("Other Settings")] 
    public float rotateTowardsSpeed;
    public float retargetCheckTime;

    public LayerMask enemyLayerMask;

}
