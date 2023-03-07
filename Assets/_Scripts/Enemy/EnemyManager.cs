using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PathologicalGames;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Settings")]
    public GameSettingsEnemyPrefabs settingsEnemyPrefabs;

    public List<Enemy> spawnedEnemies;
    public GroupManager groupManager;
    
    void Start()
    {
        GameHub.Instance.eventsManager.OnCharactersPositionChanged += PingEnemiesToRetarget;
        GameHub.Instance.eventsManager.OnCharacterLeave += RetargetEnemies;
        GameHub.Instance.eventsManager.OnAllCharactersDead += SetEnemiesToIdle;
    }
    
    void OnDestroy()
    {
        GameHub.Instance.eventsManager.OnCharactersPositionChanged -= PingEnemiesToRetarget;
        GameHub.Instance.eventsManager.OnCharacterLeave -= RetargetEnemies;
        GameHub.Instance.eventsManager.OnAllCharactersDead -= SetEnemiesToIdle;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);
        groupManager.RemoveTarget(enemy);
    }

    public void RetargetEnemies(object data)
    {
        var target = (BaseCharacter)data;
        
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy.CurrentEnemyState is EnemyStates.Idle) continue;
            if (enemy.CurrentEnemyState is EnemyStates.Dead) continue;
            
            enemy.SetNewTarget(target);
        }
    }

    public bool HasPossibleTargets()
    {
        return groupManager.GetCharacters().Count >= 1;
    }

    public List<BaseCharacter> GetPossibleTargets()
    {
        return groupManager.GetCharacters();
    }

    public void SetEnemiesToIdle(object data)
    {
        foreach (var enemy in spawnedEnemies)
        {
            enemy.ChangeState(EnemyStates.Idle);
        }
    }

    public void PingEnemiesToRetarget()        
    {
        foreach (var enemy in spawnedEnemies)
        {
            enemy.TriggerRetarget = true;
        }
    }
}
