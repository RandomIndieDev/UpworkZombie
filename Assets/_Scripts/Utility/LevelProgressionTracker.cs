using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionTracker : MonoBehaviour
{
    private GameSettings gameSettings;
    private GameSettingsCharacters gameSettingsCharacters;
    private GameSettingsEnemyPrefabs gameSettingsEnemyPrefabs;
    private GameSettingsLevelProgression gameSettingsLevelProgression;

    private EventsManager m_EventManager;

    private int m_EnemiesKilledCount;
    private int m_DistanceTravelled;
    
    public int EnemiesKilledCount
    {
        get => m_EnemiesKilledCount;
        set => m_EnemiesKilledCount = value;
    }
    
    public int DistanceTravelled
    {
        get => m_DistanceTravelled;
        set => m_DistanceTravelled = value;
    }

    public List<float> expLevels;
    
    public void Init(GameSettings gameSettings, GameSettingsCharacters gameSettingsCharacters, GameSettingsEnemyPrefabs enemyPrefabs, GameSettingsLevelProgression gameSettingsLevelProgression)
    {
        this.gameSettings = gameSettings;
        this.gameSettingsCharacters = gameSettingsCharacters;
        this.gameSettingsEnemyPrefabs = enemyPrefabs;
        this.gameSettingsLevelProgression = gameSettingsLevelProgression;

        m_EnemiesKilledCount = 0;
        m_DistanceTravelled = 0;

        m_EventManager = GameHub.Instance.eventsManager;
        
        LevelXPSetUp();
    }

    public float GetCharacterStatIncreasePercentage()
    {
        return gameSettingsLevelProgression.m_StatIncreasePercentage + Random.Range(
                   -gameSettingsLevelProgression.m_StatDifferencePercentage,
                   gameSettingsLevelProgression.m_StatDifferencePercentage);
    }

    public float GetHealStatIncreasePercentage(int level)
    {
        var increaseVariance = gameSettingsLevelProgression.m_SpecialAbilityDifferencePercentage * level;
        var randPercentage = Random.Range(-increaseVariance, increaseVariance);

        return (gameSettingsLevelProgression.m_HealerHealPercentageIncrease * level) + randPercentage;
    }

    public void OnEnable()
    {
        m_EventManager.OnEnemyDied += IncrementEnemyKilled;
        UIEvents.Instance.OnDistanceTick += IncrementDistance;
    }

    public void OnDisable()
    {
        m_EventManager.OnEnemyDied -= IncrementEnemyKilled;
        UIEvents.Instance.OnDistanceTick -= IncrementDistance;
    }
    
    public StartingCharacterStats GetMeleeStats()
    {
        var stats = new StartingCharacterStats();
        
        stats.health = gameSettingsCharacters.startingMeleeHealth;
        stats.damage = gameSettingsCharacters.startingMeleeDamage;
        stats.attackRange = gameSettingsCharacters.meleeAttackRange;

        return stats;
    }
    
    public StartingCharacterStats GetHandgunStats()
    {
        var stats = new StartingCharacterStats();
        
        stats.health = gameSettingsCharacters.startingHandgunHealth;
        stats.damage = gameSettingsCharacters.startingHandgunDamage;
        stats.attackRange = gameSettingsCharacters.startingHandgunRange;

        return stats;
    }
    
    public StartingCharacterStats GetSniperStats()
    {
        var stats = new StartingCharacterStats();
        
        stats.health = gameSettingsCharacters.startingSniperHealth;
        stats.damage = gameSettingsCharacters.startingSniperDamage;
        stats.attackRange = gameSettingsCharacters.startingSniperRange;

        return stats;
    }

    private void IncrementEnemyKilled(object data)
    {
        m_EnemiesKilledCount += 1;
    }

    private void IncrementDistance(object data)
    {
        m_DistanceTravelled += 1;
    }
    
    public int GetEnemySpawnNumber()
    {
        var increaseAmt = (m_DistanceTravelled / 10) * gameSettingsEnemyPrefabs.spawnIncreaseAmtPer10Meters;
        var spawnAmt = Random.Range(gameSettingsEnemyPrefabs.startingPossibleSpawnAmt.x + increaseAmt, gameSettingsEnemyPrefabs.startingPossibleSpawnAmt.y + increaseAmt);

        return spawnAmt;
    }

    public float GetCurrentExpGain()
    {
        var expGain = (float) gameSettings.startExpValue;
        expGain += Random.Range(-(gameSettings.startExpValue * gameSettings.expVariancePercentage), gameSettings.startExpValue * gameSettings.expVariancePercentage);
        return expGain;
    }

    public float GetCommanderEffectDamage(int level)
    {
        level -= 1;
        return gameSettingsCharacters.baseCommanderDamageIncrease * (level * gameSettingsLevelProgression.m_CommanderDamageBuffIncrease);
    }

    public float GetCurrentRequiredExp(int level)
    {
        return expLevels[level];
    }

    public bool HasPassedLevel(int currentLevel, float value)
    {
        return value > expLevels[currentLevel];
    }
    
    void LevelXPSetUp(){
        for (int i = 1; i < expLevels.Count; i++) {
            expLevels[i] = (int)(Mathf.Floor(25*(Mathf.Pow(i,1.8f))));
        }
    }
}

public struct StartingCharacterStats
{
    public float health;
    public float damage;
    public float attackRange;
}
