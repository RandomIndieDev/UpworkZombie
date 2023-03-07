using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsEnemies", menuName = "Game Settings/Enemies", order = 1)]
public class GameSettingsEnemiesType : ScriptableObject
{
    [Header("Enemy Settings")] 
    public float basicStartingHealth;
    public float basicStartingDamage;
    public float basicMoveSpeed;
    [Space(10)]
    public float rotateSpeed;
    public float attackDistance;
}
