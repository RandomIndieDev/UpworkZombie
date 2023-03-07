using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using Lofelt.NiceVibrations;
using UnityEngine;

[RequireComponent(typeof(CharacterPathFollower))]
public class GroupManager : MonoBehaviour
{
    private GameSettings gameSettings;
    private GameSettingsCharacters gameSettingsCharacters;

    [Header("References")] 
    public Transform survivorParent;
    public GroupPositionSwitcher groupPositionSwitcher;

    public List<ICombat> currentTargets;
    private List<IAlly> groupAllys;
    private List<BaseCharacter> characters;

    private CharacterPathFollower m_PathFollower;

    private List<BaseCharacter> newlyFoundSurvivors;

    private bool m_GameStarted;

    public void SetCollidingEnemies(int value)
    {
        m_PathFollower.HeldBackValue += value;
    }
    
    void Awake()
    {
        gameSettings = GameHub.Instance.gameSettings;
        gameSettingsCharacters = GameHub.Instance.gameSettingsCharacters;
        
        groupAllys = new List<IAlly>();
        characters = new List<BaseCharacter>();
        newlyFoundSurvivors = new List<BaseCharacter>();
        currentTargets = new List<ICombat>();
        m_PathFollower = GetComponent<CharacterPathFollower>();
        
        SetupStartingCharacters();
    }

    void SetupStartingCharacters()
    {
        for (var i = 0; i < gameSettings.startingCharacterAmt; i++)
        {
            var startingCharacter = Instantiate(GameHub.Instance.GetRandomSurvivor(), survivorParent);
            AddSurvivorToGroup(startingCharacter.GetComponent<BaseCharacter>());
        }
    }

    void OnEnable()
    {
        GameHub.Instance.eventsManager.OnGameStarted += StartMovement;
        GameHub.Instance.eventsManager.OnAllCharactersDead += StopMovement;
        GameHub.Instance.eventsManager.OnHealAuraTriggered += HealGroup;
        GameHub.Instance.eventsManager.OnEnemyEnterGroupRange += IncrementSlowDownCount;
        GameHub.Instance.eventsManager.OnEnemyLeaveGroupRange += DecrementSlowDownCount;
        
        GameHub.Instance.eventsManager.OnSwipeActionStart += StopMovement;
        GameHub.Instance.eventsManager.OnSwipeActionEnded += StartMovement;
        
    }

    void OnDisable()
    {
        GameHub.Instance.eventsManager.OnGameStarted -= StartMovement;
        GameHub.Instance.eventsManager.OnAllCharactersDead -= StopMovement;
        GameHub.Instance.eventsManager.OnHealAuraTriggered -= HealGroup;
        GameHub.Instance.eventsManager.OnEnemyEnterGroupRange -= IncrementSlowDownCount;
        GameHub.Instance.eventsManager.OnEnemyLeaveGroupRange -= DecrementSlowDownCount;
        
        GameHub.Instance.eventsManager.OnSwipeActionStart -= StopMovement;
        GameHub.Instance.eventsManager.OnSwipeActionEnded -= StartMovement;
    }

    private void StartMovement(object data)
    {
        StartMovement();
    }

    private void StartMovement()
    {
        m_GameStarted = true;
        
        foreach (var ally in groupAllys)
        {
            ally.ChangeState(CharacterStates.Run);
        }
        
        m_PathFollower.StartMovement();
    }

    private void StopMovement(object data)
    {
        foreach (var ally in groupAllys)
        {
            ally.ChangeState(CharacterStates.Idle);
        }
        
        m_PathFollower.StopMovement();
    }

    public void NewSurvivorsFound(NewSurvivorGroup newSurvivorGroup)
    {
        newSurvivorGroup.InitiateFound();
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        
        if (characters.Count >= gameSettings.maxGroupSize) return;

        newlyFoundSurvivors.AddRange(newSurvivorGroup.m_NewSurvivors);
        
        UIEvents.Instance.DoNewSurvivorsFound(newSurvivorGroup.m_NewSurvivors.Count);
        GameHub.Instance.eventsManager.DoCutsceneEventHappened();
    }
    
    public void AddTarget(ICombat enemy)
    {
        currentTargets.Add(enemy);
        UpdateAllysWithTargets();
    }

    public void UpdateAllysWithTargets()
    {
        foreach (var ally in groupAllys)
        {
            ally.UpdateClosestTarget();
        }
    }

    public List<BaseCharacter> GetCharacters()
    {
        return characters;
    }

    public void RemoveTarget(ICombat enemy)
    {
        currentTargets.Remove(enemy);

        if (currentTargets.Count >= 0)
        {
            UpdateAllysWithTargets();
        }
    }

    private void AddSurvivorToGroup(BaseCharacter character)
    {
        m_PathFollower.CurrentGroupSizeValue += 1;
        
        var ally = character.GetComponent<IAlly>();

        ally.SubscribeToGroup(this);
        
        ally.ChangeState(m_GameStarted ? CharacterStates.Run : CharacterStates.Idle);
        
        groupAllys.Add(ally);
        characters.Add(character);
        
        groupPositionSwitcher.AddToFreePosition(character);
    }

    public void RemoveSurvivorFromGroup(BaseCharacter character)
    {
        m_PathFollower.CurrentGroupSizeValue -= 1;

        character.IsInGroup = false;
        
        var ally = character.GetComponent<IAlly>();
        
        groupAllys.Remove(ally);
        characters.Remove(character);

        if (groupAllys.Count > 0) return;
        
        var data = new LevelEndStats(GameHub.Instance.levelProgressionTracker.EnemiesKilledCount, GameHub.Instance.levelProgressionTracker.DistanceTravelled);
        GameHub.Instance.eventsManager.DoAllCharactersDead(data);
    }

    public int GetCurrentSurvivorCount()
    {
        return characters.Count;
    }

    public void HealGroup(object data)
    {
        var healAmt = (float) data;

        foreach (var ally in groupAllys)
        {
            ally.HealFlatAmount(healAmt);
        }
        
    }

    public bool CanAcceptCharacters()
    {
        return characters.Count + newlyFoundSurvivors.Count <= gameSettings.maxGroupSize;
    }
    
    private void IncrementSlowDownCount(object data)
    {
        m_PathFollower.HeldBackValue += 1;
    }

    private void DecrementSlowDownCount(object data)
    {
        m_PathFollower.HeldBackValue -= 1;
    }

    #region SurvivorAcceptance

    public void DeclineNewSurvivors()
    {
        UIEvents.Instance.DoPlayerDoneSelecting();
        
        RespondToNewSurvivors(false);
    }
    
    public void RespondToNewSurvivors(bool isAccepted)
    {
        
        if (isAccepted)
            UIEvents.Instance.DoNewPlayerJoined(newlyFoundSurvivors.Count);
        
        foreach (var survivor in newlyFoundSurvivors)
        {

            if (isAccepted)
            {
                EffectsManager.Instance.DoSurvivorReactionEffect(SurvivorReactions.Happy, survivor.transform.position, new Vector3(0,4.5f,0), survivor.transform);
                
                survivor.transform.parent = survivorParent.transform;
                
                AddSurvivorToGroup(survivor);
            
            }
            else
            {
                SurvivorReactions reaction;
                reaction = Random.value <= GameHub.Instance.gameSettingsCharacters.survivorRejectedAggroChance ? SurvivorReactions.Angry : SurvivorReactions.Sad;
                
                EffectsManager.Instance.DoSurvivorReactionEffect(reaction, survivor.transform.position, new Vector3(0,4.5f,0), transform);
            }

        }

        newlyFoundSurvivors = new List<BaseCharacter>();

        GameHub.Instance.eventsManager.DoCutsceneEventEnded();
    }

    #endregion

}
