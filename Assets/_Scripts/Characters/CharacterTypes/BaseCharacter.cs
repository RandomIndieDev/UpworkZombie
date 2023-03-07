using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class BaseCharacter : MonoBehaviour, IAlly, ICombat
{
    [Header("Game Settings")] 
    private GameSettings gameSettings;
    private GameSettingsCharacters gameSettingsCharacters;
    private LevelProgressionTracker m_LevelProgressionTracker;

    [Header("Character Stats")] 
    public CharacterStats characterStats;
    
    [Header("Current Stats")] 
    private float maxHealth;

    private float attackRange;
    protected float currentDamage;
    private int commanderEffectDamage;
    
    protected CharacterHealthBarHandler healthBarHandler;
    protected CharacterAnimationHandler animationHandler;
    protected CharacterAttributeHandler attributeHandler;
    protected GoToTargetBehaviour goToTargetBehaviour;
    protected CharacterSelector characterSelector;
    
    protected ICombat closestTarget;
    protected bool isAttacking;
    
    protected GameObject shootLoc;
    
    protected List<ICombat> possibleTargets;

    protected GroupManager groupManager;

    private float currentHealth;
    private float tick;
    private float healTick;
    
    private bool isAlive;
    private bool isInGroup;

    private bool triggerAttack;
    private bool hasCommanderEffect;
    private int currentContacts;
    
    private StateMachine stateMachine = new StateMachine();

    #region Accessor Methods
    public CharacterHealthBarHandler HealthBarHandler => healthBarHandler;
    public CharacterAnimationHandler AnimationHandler => animationHandler;

    public CharacterAttributeHandler AttributeHandler => attributeHandler;
    public GoToTargetBehaviour GoToTargetBehaviour => goToTargetBehaviour;
    public GameSettings GameSettings => gameSettings;
    public GameSettingsCharacters GameSettingsCharacters => gameSettingsCharacters;

    private int m_WeaponTypeInt = 0;

    public int WeaponTypeInt
    {
        get => m_WeaponTypeInt;
        set => m_WeaponTypeInt = value;
    }
    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }
    
    public GroupManager GroupManager
    {
        get => groupManager;
        set => groupManager = value;
    }
    
    public ICombat ClosestTarget
    {
        get => closestTarget;
        set => closestTarget = value;
    }
    
    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }
    
    public bool TriggerAttack
    {
        get => triggerAttack;
        set => triggerAttack = value;
    }
    
    public bool HasCommanderEffect
    {
        get => hasCommanderEffect;
        set => hasCommanderEffect = value;
    }
    
    public bool IsInGroup
    {
        get => isInGroup;
        set => isInGroup = value;
    }


    #endregion

    public abstract void DoAttackStart();
    public abstract void DoAttack();

    protected void Init(StartingCharacterStats stats, CharacterType charType)
    {
        
        GetReferences();
        
        attributeHandler.Init(gameSettingsCharacters);
        healthBarHandler.Init(gameSettings);
        
        characterSelector.SetupWeapon(charType);

        if (charType == CharacterType.Handgun || charType == CharacterType.Sniper)
        {
            shootLoc = characterSelector.GetRangedWeaponShootPoint();

            if (charType == CharacterType.Handgun)
                m_WeaponTypeInt = 1;

            if (charType == CharacterType.Sniper)
                m_WeaponTypeInt = 5;
        }
        
        var charName = GameHub.Instance.characterNameGenerator.GetRandomName();
        var charAtr = GameHub.Instance.characterAttributeGenerator.GetRandomAttribute();
        
        attributeHandler.SetAttributeImage(charAtr);
        
        maxHealth = stats.health;
        currentHealth = maxHealth;
        currentDamage = stats.damage;
        attackRange = stats.attackRange;
        
        characterStats = new CharacterStats(charName, 1, 0, (int) maxHealth, (int) currentDamage, charType, charAtr);
        isAlive = true;

        StartCoroutine(RegainHealth());
        
        stateMachine.ChangeState(new RunnerIdleState(this));
    }

    void OnEnable()
    {
        GameHub.Instance.eventsManager.OnEnemyDied += GetExp;
    }

    void OnDisable()
    {
        GameHub.Instance.eventsManager.OnEnemyDied -= GetExp;
    }

    private void GetExp(object data)
    {
        if (!isAlive) return;
        if (!isInGroup) return;
        
        characterStats.CharExp += GameHub.Instance.levelProgressionTracker.GetCurrentExpGain();
        
        UIEvents.Instance.DoUpdateCharacterStats(characterStats);

        if (!GameHub.Instance.levelProgressionTracker.HasPassedLevel(characterStats.CharLevel, characterStats.CharExp)) return;

        IncreasePlayerStats();
        
        EffectsManager.Instance.DoLevelUpEffect(transform.position, transform);
        UIEvents.Instance.DoPlayerLevelUp(characterStats);
    }

    private void IncreasePlayerStats()
    {
        characterStats.CharDmg += (int) (characterStats.CharDmg * m_LevelProgressionTracker.GetCharacterStatIncreasePercentage());
        characterStats.CharHp += (int) (characterStats.CharHp * m_LevelProgressionTracker.GetCharacterStatIncreasePercentage());
        characterStats.CharLevel += 1;

        currentDamage = characterStats.CharDmg + commanderEffectDamage;
        maxHealth = characterStats.CharHp;
    }
    
    private void GetReferences()
    {
        gameSettings = GameHub.Instance.gameSettings;
        gameSettingsCharacters = GameHub.Instance.gameSettingsCharacters;
        m_LevelProgressionTracker = GameHub.Instance.levelProgressionTracker;

        healthBarHandler = GetComponent<CharacterHealthBarHandler>();
        animationHandler = GetComponent<CharacterAnimationHandler>();
        attributeHandler = GetComponent<CharacterAttributeHandler>();
        characterSelector = GetComponent<CharacterSelector>();
        goToTargetBehaviour = GetComponent<GoToTargetBehaviour>();
    }
    
    #region UNITY METHODS

    protected void Update()
    {
        stateMachine.Update();
    }
    
    #endregion

    public void DoHealEffect()
    {
        if (!characterStats.CharAtr.Equals(CharacterAttribute.Medic)) return;

        healTick += Time.deltaTime;
        
        if (!(healTick >= gameSettingsCharacters.healAuraTickRate)) return;
        
        healTick = 0;
        EffectsManager.Instance.DoHealEffect(transform.position, new Vector3(0,.5f,0), transform);
        GameHub.Instance.eventsManager.DoHealAuraTriggered(gameSettingsCharacters.baseHealAura + m_LevelProgressionTracker.GetHealStatIncreasePercentage(characterStats.CharLevel));

    }

    public void AddCommanderEffectDamage(float damage)
    {
        commanderEffectDamage = (int) damage;
        
        characterStats.CharExtraDmg = commanderEffectDamage;
        currentDamage += commanderEffectDamage;
        hasCommanderEffect = true;
    }

    public void RemoveCommanderEffectDamage()
    {
        characterStats.CharExtraDmg = 0;
        currentDamage -= commanderEffectDamage;
        hasCommanderEffect = false;
    }

    public void DoTargetUpdate()
    {
        tick += Time.deltaTime;

        if (!(tick >= gameSettingsCharacters.retargetCheckTime)) return;
        if (groupManager == null) return;
        
        UpdateClosestTarget();

        tick = 0;
    }
    
    public bool UpdateClosestTarget()
    {
        if (groupManager == null) return false;
        
        var distance = Mathf.Infinity;
        var targetLocated = false;

        foreach (var target in groupManager.currentTargets)
        {
            var calculatedDis = Vector3.Distance(target.GetCurrentTransform().position, transform.position);

            if (!(calculatedDis < distance)) continue;

            closestTarget = target;

            targetLocated = true;
            distance = calculatedDis;
        }
        
        return targetLocated;
    }

    public void HealFlatAmount(float healAmt)
    {
        IncreaseHealth((int) healAmt);
    }

    public ICombat GetClosestTarget()
    {
        return closestTarget;
    }

    public void SubscribeToGroup(GroupManager manager)
    {
        groupManager = manager;
        IsInGroup = true;
    }
    IEnumerator RegainHealth()
    {
        while (isAlive)
        {
            if (currentHealth >= maxHealth) yield return null;
            
            yield return new WaitForSeconds(gameSettingsCharacters.passiveHealTickRate);
            
            IncreaseHealth(gameSettingsCharacters.passiveHealPercentage);
        }
    }

    public void IncreaseHealth(int amt)
    {
        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
        healthBarHandler.ChangeHealth(currentHealth / maxHealth);
    }

    public void IncreaseHealth(float percentage)
    {
        currentHealth = Mathf.Clamp(currentHealth + (maxHealth * percentage), 0, maxHealth);
        healthBarHandler.ChangeHealth(currentHealth / maxHealth);
    }

    public bool IsInRangeOfTarget()
    {
        return Mathf.Abs(Vector3.Distance(transform.position, closestTarget.GetCurrentTransform().position)) < attackRange;
    }
    
    protected void TurnStraight()
    {
        transform.DOLocalRotate(new Vector3(0,0,0), gameSettingsCharacters.rotateTowardsSpeed);
    }

    public void SetTargets(List<ICombat> targets)
    {
        possibleTargets = targets;
        UpdateClosestTargetFromList();
        goToTargetBehaviour.enabled = true;
    }
    
    private void UpdateClosestTargetFromList()
    {
        var distance = Mathf.Infinity;

        var targetLocated = false;
        
        foreach (var target in possibleTargets)
        {
            var calculatedDis = Vector3.Distance(target.GetCurrentTransform().position, transform.position);
            if (!(calculatedDis < distance)) continue;
        
            closestTarget = target;
            distance = calculatedDis;
        }
    }

    public bool RemoveFromGroup()
    {
        if (groupManager.GetCurrentSurvivorCount() <= 1) return false;
        
        groupManager.RemoveSurvivorFromGroup(this);
        transform.parent = null;
        
        animationHandler.StopRun();
        
        EffectsManager.Instance.DoSurvivorReactionEffect(SurvivorReactions.Cry, transform.position, new Vector3(0,4.5f,0), transform);

        GameHub.Instance.eventsManager.DoOnCharacterLeave(this);
        
        Destroy(gameObject, 10f);

        return true;
    }
    public Transform GetCurrentTransform()
    {
        return transform;
    }

    public bool ReceiveDamage(float damage, Vector3 hitOrigin)
    {
        currentHealth -= damage;
        healthBarHandler.ChangeHealth(currentHealth / maxHealth);
        
        EffectsManager.Instance.DoSurvivorHitEffect(transform.position + hitOrigin.normalized);

        characterSelector.ActiveRenderer.material.DOColor(gameSettingsCharacters.survivorHitColor, gameSettingsCharacters.survivorHitColorDuration).onComplete += () =>
            {
                characterSelector.ActiveRenderer.material.DOColor(Color.white, .1f);
            };

        if (!(currentHealth <= 0)) return false;
        
        ChangeState(CharacterStates.Dead);
        
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
        
        return true;
    }

    public bool IsACommander()
    {
        return characterStats.CharAtr == CharacterAttribute.Commander;
    }
    
    public bool IsAMedic()
    {
        return characterStats.CharAtr == CharacterAttribute.Medic;
    }
    public bool IsDead()
    {
        return !isAlive;
    }

    public void PrintDetails()
    {
        throw new NotImplementedException();
    }

    #region Statemachine Method
    public void ChangeState(CharacterStates state)
    {
        switch (state)
        {
            case CharacterStates.Idle:
                stateMachine.ChangeState(new RunnerIdleState(this));
                break;
            case CharacterStates.Run:
                stateMachine.ChangeState(new RunnerRunState(this));
                break;
            case CharacterStates.Attack:
                stateMachine.ChangeState(new RunnerAttackState(this));
                break;
            case CharacterStates.Dead:
                stateMachine.ChangeState(new RunnerDeadState(this));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    
    #endregion
}

public enum CharacterStates
{
    Idle,
    Run,
    Attack,
    Dead
}

