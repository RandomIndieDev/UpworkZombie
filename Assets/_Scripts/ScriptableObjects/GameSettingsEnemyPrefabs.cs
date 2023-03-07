using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsEnemyPrefabs", menuName = "Game Settings/EnemyPrefabs", order = 1)]
public class GameSettingsEnemyPrefabs : ScriptableObject
{
    [Header("Enemy Types")] 
    public GameObject basicEnemy;
    public GameObject largeEnemy;

    [Header("Spawn Settings")] 
    public Vector2Int startingPossibleSpawnAmt;
    public int spawnIncreaseAmtPer10Meters;

    [Header("Enemy Effects")] 
    public Color slowDownEffect;
    public Color hitEffect;

    [Header("Enemy Mob Settings")] 
    public AnimationCurve m_EnemySpawnRateCurve;
    [Space(10)]
    public Vector2Int m_EnemySpawnAmtMinMax;
    public Vector2 m_EnemySpawnTickStartEnd;
    public int m_MaxDistanceTillMaxValues;
    
    public float GetNextEnemySpawnTick(int distanceTravelled)
    {
        if (distanceTravelled >= m_MaxDistanceTillMaxValues)
            distanceTravelled = m_MaxDistanceTillMaxValues;

        return Mathf.Lerp(m_EnemySpawnTickStartEnd.x,
            m_EnemySpawnTickStartEnd.y,
            m_EnemySpawnRateCurve.Evaluate((float)distanceTravelled / m_MaxDistanceTillMaxValues));
    }
    public int GetEnemySpawnAmt(int distanceTravelled)
    {
        if (distanceTravelled >= m_MaxDistanceTillMaxValues)
            distanceTravelled = m_MaxDistanceTillMaxValues;
        
        return (int) Mathf.Lerp(m_EnemySpawnAmtMinMax.x, 
            m_EnemySpawnAmtMinMax.y,
            m_EnemySpawnRateCurve.Evaluate((float) distanceTravelled / m_MaxDistanceTillMaxValues));
    }
    

}
