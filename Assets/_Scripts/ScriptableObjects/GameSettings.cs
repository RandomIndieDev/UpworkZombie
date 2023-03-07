using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/Main", order = 1)]
public class GameSettings : ScriptableObject
{

    [Header("--RUNTIME-- Game Values")] 
    public int m_GroupTravelledDistance;

    [Header("New Survivor Spawn Settings")]
    public float spawnAboveThreshold;
    public float spawnBelowThreshold;
    [Space(5)] 
    public int startingCharacterAmt;
    public int maxGroupSize;
    
    [Header("Group Movement Settings")] 
    public float maxGroupMoveSpeed;
    public float minGroupMoveSpeed;
    public int maxGroupHeldBackAmt;
    public float heldBackSpeedDecrease;
    public float heldBackMinSpeedDecreasePercentage;

    [Header("Group Selection Options")] 
    public float minSplitDistance;
    public float swipeShowDuration;
    public LayerMask groupSelectionMask;
    public float minPlayerClickTimeForMove;

    [Header("Group Distance Calculation Settings")]
    public float speedPercentageMultiplier;
    public float secondsToMeter;

    [Header("Health Bar")] 
    public float healthValueToSpeed;

    [Header("UI Settings")] 
    public float autoHideNotificationsDelay;

    [Header("Exp Settings")] 
    public int startExpValue;
    public float expVariancePercentage;





}
