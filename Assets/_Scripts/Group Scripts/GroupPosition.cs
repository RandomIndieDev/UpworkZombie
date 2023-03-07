using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Doozy.Engine.Nody.Nodes;
using UnityEngine;

public class GroupPosition : MonoBehaviour
{
    [Header("Settings")]
    public int m_LocationIndex;
    public List<int> adjacentPositions;
    
    [Header("References")]
    public Transform m_LocationTransform;
    public GameObject indicatorEffect;
    public GameObject commanderEffect;
    
    [Header("Parented Character")]
    public BaseCharacter character;
    
    private bool hasCommander;
    private int commanderEffectLevel; 
    
    private bool hasHealer;
    
    private bool commanderEffectActive;

    private int commanderAuraCount;

    void Awake()
    {
        commanderAuraCount = 0;
    }

    void OnEnable()
    {
        GameHub.Instance.eventsManager.OnCommanderAuraTriggered += ActivateCommanderEffect;
        GameHub.Instance.eventsManager.OnCommanderAuraRemoved += DeactivateCommanderEffect;
        GameHub.Instance.eventsManager.OnCharacterDied += RemoveCharacterIfDead;
        
    }

    void OnDisable()
    {
        GameHub.Instance.eventsManager.OnCommanderAuraTriggered -= ActivateCommanderEffect; 
        GameHub.Instance.eventsManager.OnCommanderAuraRemoved -= DeactivateCommanderEffect; 
        GameHub.Instance.eventsManager.OnCharacterDied -= RemoveCharacterIfDead; 
    }
    
    public void MoveCharacterUp(GameObject silhouette)
    {
        silhouette.SetActive(true);
        silhouette.transform.position = character.transform.position;
        silhouette.transform.DOLocalMove(silhouette.transform.localPosition + new Vector3(0, 2, 0), .3f);
    }
    public bool IsBeingUsed()
    {
        return character != null;
    }

    public void ResetSpot()
    {
        commanderEffect.SetActive(false);
        
        RemoveCommanderAura();

        if (character == null) return;
        
        if (character.HasCommanderEffect)
        {
            character.HasCommanderEffect = false;
            character.RemoveCommanderEffectDamage();
        };
        
        character = null;
        
    }
    public void AddCharacterToPos(BaseCharacter newChar)
    {
        
        if (newChar == null) return;
            
        character = newChar;
        
        character.transform.DOLocalMove(m_LocationTransform.localPosition, .3f);
        character.transform.DOLocalRotate(new Vector3(0, 0, 0), .3f);
        
        if (character.IsACommander())
        {
            TriggerCommanderAura(character.characterStats.CharLevel);
            return;
        }

        if (commanderAuraCount <= 0)
        {
            if (!character.HasCommanderEffect) return;
            
            character.HasCommanderEffect = false;
            character.RemoveCommanderEffectDamage();

            return;
        };
        
        if (character.HasCommanderEffect) return;

        character.AddCommanderEffectDamage(GameHub.Instance.levelProgressionTracker.GetCommanderEffectDamage(commanderEffectLevel));
        character.HasCommanderEffect = true;
        commanderEffect.SetActive(true);
    }

    public void ResetCharacterToPosition()
    {
        character.transform.DOLocalMove(m_LocationTransform.localPosition, .3f);
    }

    private void ActivateCommanderEffect(object data)
    {
        var effectData = (AuraTriggeredInfo) data;

        if (!effectData.adjPositions.Contains(m_LocationIndex)) return;
        
        commanderAuraCount += 1;
        commanderEffectLevel = effectData.charLevel;

        if (character == null) return;
        if (character.IsACommander()) return;
        
        commanderEffect.SetActive(true);
        commanderEffectActive = true;
        
        if (character.HasCommanderEffect) return;
        
        character.AddCommanderEffectDamage(GameHub.Instance.levelProgressionTracker.GetCommanderEffectDamage(effectData.charLevel));
        character.HasCommanderEffect = true;
    }

    private void DeactivateCommanderEffect(object data)
    {
        var indexList = (List<int>) data;

        if (!indexList.Contains(m_LocationIndex)) return;

        commanderAuraCount -= 1;
        
        if (commanderAuraCount > 0) return;
        
        commanderEffect.SetActive(false);
        commanderEffectActive = false;

        if (character == null) return;
        if (!character.HasCommanderEffect) return;
        
        character.RemoveCommanderEffectDamage();
        character.HasCommanderEffect = false;
    }

    private void RemoveCharacterIfDead()
    {
        if (character == null) return;
        if (character.IsAlive) return;
        
        ResetSpot();
    }

    public void SetupIndicator(bool enable)
    {
        indicatorEffect.gameObject.SetActive(enable);
    }

    private void TriggerCommanderAura(int level)
    {
        hasCommander = true;

        GameHub.Instance.eventsManager.DoCommanderAuraTriggered(new AuraTriggeredInfo(adjacentPositions, level));
    }

    private void RemoveCommanderAura()
    {
        if (!hasCommander) return;
        
        hasCommander = false;
        GameHub.Instance.eventsManager.DoCommanderAuraRemoved(adjacentPositions);
        
    }
    
    public struct AuraTriggeredInfo
    {
        public List<int> adjPositions;
        public int charLevel;

        public AuraTriggeredInfo(List<int> adjPositions, int charLevel)
        {
            this.adjPositions = adjPositions;
            this.charLevel = charLevel;
        }
    }
}

