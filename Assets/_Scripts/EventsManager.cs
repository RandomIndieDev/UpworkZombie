using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public event Action OnNewSectionSpawned;
    public event Action OnCutsceneEventHappened;
    public event Action OnCutsceneEventEnded;

    public event Action<object> OnSwipeAction;
    public event Action<object> OnSwipeActionStart;
    public event Action<object> OnSwipeActionEnded;
    
    public event Action OnCharactersPositionChanged;

    public event Action OnCharacterDied;
    public event Action<object> OnCharacterLeave;
    public event Action<object> OnAllCharactersDead;

    public event Action OnGameStarted;

    public event Action<object> OnHealAuraTriggered;
    public event Action<object> OnCommanderAuraTriggered;
    public event Action<object> OnCommanderAuraRemoved;

    public event Action<object> OnEnemyDied;
    public event Action<object> OnEnemyEnterGroupRange;
    public event Action<object> OnEnemyLeaveGroupRange;

    public event Action<object> OnTestMe;

    public void DoOnCharacterLeave(object data)
    {
        OnCharacterLeave?.Invoke(data);
    }
    public void DoOnSwipeAction(object data)
    {
        OnSwipeAction?.Invoke(data);
    }
    
    public void DoOnSwipeActionStart(object data)
    {
        OnSwipeActionStart?.Invoke(data);
    }
    
    public void DoOnSwipeActionEnded(object data)
    {
        OnSwipeActionEnded?.Invoke(data);
    }
    
    public void DoOnTest(object data)
    {
        OnTestMe?.Invoke(data);
    }
    public void DoNewSectionSpawned()
    {
        OnNewSectionSpawned?.Invoke();
    }
    
    public void DoCutsceneEventHappened()
    {
        OnCutsceneEventHappened?.Invoke();
    }
    
    public void DoCutsceneEventEnded()
    {
        OnCutsceneEventEnded?.Invoke();
    }
    
    public void DoCharacterPositionChanged()
    {
        OnCharactersPositionChanged?.Invoke();
    }

    public void DoCharacterDied()
    {
        OnCharacterDied?.Invoke();
    }
    
    public void DoHealAuraTriggered(object data)
    {
        OnHealAuraTriggered?.Invoke(data);
    }

    public void DoCommanderAuraTriggered(object data)
    {
        OnCommanderAuraTriggered?.Invoke(data);
    }
    
    public void DoCommanderAuraRemoved(object data)
    {
        OnCommanderAuraRemoved?.Invoke(data);
    }
    
    public void DoAllCharactersDead(object data)
    {
        UIEvents.Instance.DoAllPlayersDead(data);
        OnAllCharactersDead?.Invoke(data);
    }
    
    public void DoGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public void DoEnemyDied(object data)
    {
        OnEnemyDied?.Invoke(data);
    }
    
    public void DoEnemyEnterGroupRange(object data)
    {
        OnEnemyEnterGroupRange?.Invoke(data);
    }
    
    public void DoEnemyLeaveGroupRange(object data)
    {
        OnEnemyLeaveGroupRange?.Invoke(data);
    }
    
}
