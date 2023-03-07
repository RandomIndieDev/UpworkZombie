using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    private static UIEvents _instance;

    public static UIEvents Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    public event Action<object> OnDistanceTick;
    public event Action<object> OnNewSurvivorsFound;

    public event Action OnPlayerDoneSelecting;
    
    public event Action<GameObject> OnRenderCharacter;
    
    public event Action<CharacterStats> OnShowCharacterStats;
    public event Action<CharacterStats> OnUpdateCharacterStats;

    public event Action<CharacterStats> OnPlayerLevelUp;
    public event Action<CharacterStats> OnPlayerDeath;
    public event Action<int> OnNewPlayersJoined;

    public event Action<object> OnAllPlayersDead;
    

    public void DoDistanceTick(object data)
    {
        OnDistanceTick?.Invoke(data);
    }
    
    public void DoNewSurvivorsFound(object data)
    {
        OnNewSurvivorsFound?.Invoke(data);
    }
    
    public void DoPlayerDoneSelecting()
    {
        OnPlayerDoneSelecting?.Invoke();
    }

    public void DoShowCharacterStats(CharacterStats stats)
    {
        OnShowCharacterStats?.Invoke(stats);
    }
    
    public void DoRenderCharacter(GameObject character)
    {
        OnRenderCharacter?.Invoke(character);
    }


    public void DoPlayerLevelUp(CharacterStats obj)
    {
        OnPlayerLevelUp?.Invoke(obj);
    }
    
    public void DoUpdateCharacterStats(CharacterStats obj)
    {
        OnUpdateCharacterStats?.Invoke(obj);
    }
    
    public void DoPlayerDeath(CharacterStats obj)
    {
        OnPlayerDeath?.Invoke(obj);
    }
    
    public void DoAllPlayersDead(object data)
    {
        OnAllPlayersDead?.Invoke(data);
    }
    
    public void DoNewPlayerJoined(int amt)
    {
        OnNewPlayersJoined?.Invoke(amt);
    }
}
