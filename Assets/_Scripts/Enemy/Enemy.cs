using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using PathologicalGames;
using UnityEngine;

public class Enemy : MonoBehaviour, ICombat
{
    [Header("Settings")]
    public GameSettings gameSettings;
    public GameSettingsEnemiesType m_EnemyTypeSettings;
    public GameSettingsEnemyPrefabs GameSettingsEnemy;

    [Header("References")] 
    public Rigidbody rigidbody;
    public EnemyAnimationHandler animationHandler;
    public EnemyRandomizer enemyRandomizer;
    public CharacterHealthBarHandler healthBarHandler;

    public GameObject model;
    private ICombat m_CurrentTarget;
    private EnemyManager enemyManager;
    
    private float maxHealth;
    private float currentHealth;
    private float currentDamage;

    private float m_SlowDownPercentage = 0f;

    public float SlowDownPercentage
    {
        get => m_SlowDownPercentage;
        set => m_SlowDownPercentage = value;
    }

    private bool m_SlowDownActive;

    private float m_SlowDownTimeAmt;

    private float moveSpeed;
    
    private bool isMoving;
    private bool isAttacking;
    private bool isDead;
    private bool isInGroupRange;

    private bool triggerRetarget;
    private bool triggerAttack;

    private EnemyStates m_CurrentEnemyState;

    public EnemyStates CurrentEnemyState => m_CurrentEnemyState;

    private StateMachine stateMachine = new StateMachine();
    
    #region Properties

    public ICombat ClosestPlayer
    {
        get => m_CurrentTarget;
        set => m_CurrentTarget = value;
    }
    
    public Rigidbody Rigidbody
    {
        get => rigidbody;
        set => rigidbody = value;
    }
    
    public bool IsMoving
    {
        get => isMoving;
        set => isMoving = value;
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }
    
    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
    
    public StateMachine StateMachine
    {
        get => stateMachine;
        set => stateMachine = value;
    }
    
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    
    public bool IsInGroupRange
    {
        get => isInGroupRange;
        set => isInGroupRange = value;
    }
    
    public bool TriggerRetarget
    {
        get => triggerRetarget;
        set => triggerRetarget = value;
    }
    
    public bool TriggerAttack
    {
        get => triggerAttack;
        set => triggerAttack = value;
    }
    
    public float CurrentDamage
    {
        get => currentDamage;
        set => currentDamage = value;
    }
    public EnemyManager EnemyManager => enemyManager;

    #endregion

    private void Awake()
    {
        healthBarHandler.Init(gameSettings);
        GameSettingsEnemy = GameHub.Instance.gameSettingsEnemyPrefabs;
    }

    private void Start()
    {
        stateMachine.ChangeState(new EnemyIdleState(this));
    }

    private void Update()
    {
        stateMachine.Update();
        CountDownSlowAmt();
    }
    
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    public void SubscribeToEnemyManager(EnemyManager manager)
    {
        enemyManager = manager;
    }
    public Transform GetCurrentTransform()
    {
        return transform;
    }

    bool ICombat.IsDead()
    {
        return isDead;
    }

    private void CountDownSlowAmt()
    {
        if (!m_SlowDownActive) return;

        m_SlowDownTimeAmt -= Time.deltaTime;

        if (!(m_SlowDownTimeAmt <= 0)) return;
        
        m_SlowDownActive = false;
        m_SlowDownTimeAmt = 0;
        m_SlowDownPercentage = 0;
        enemyRandomizer.renderer.material.DOColor(Color.white, .1f);

    }

    #region Enemy States

    public void ChangeState(EnemyStates state)
    {

        m_CurrentEnemyState = state;
        
        switch (state)
        {
            case EnemyStates.Idle:
                stateMachine.ChangeState(new EnemyIdleState(this));
                break;
            case EnemyStates.Move:
                stateMachine.ChangeState(new EnemyMoveState(this));
                break;
            case EnemyStates.Attack:
                stateMachine.ChangeState(new EnemyAttackState(this));
                break;
            case EnemyStates.Dead:
                stateMachine.ChangeState(new EnemyDeadState(this));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        
    }
    public void DoTriggerAttack()
    {
        triggerAttack = true;
    }
    
    #endregion

    public void PrintDetails()
    {
        print(gameObject.name);
    }

    public void SetNewTarget(BaseCharacter character)
    {
        m_CurrentTarget = character.GetComponent<ICombat>();
    }

    public void DespawnUnit()
    {
        PoolManager.Pools["Enemies"].Despawn(transform);
    }

    #region PoolEvents
    
    public void OnSpawned()  
    {
        stateMachine.ChangeState(new EnemyIdleState(this));

        maxHealth = m_EnemyTypeSettings.basicStartingHealth;
        currentHealth = m_EnemyTypeSettings.basicStartingHealth;
        currentDamage = m_EnemyTypeSettings.basicStartingDamage;
        moveSpeed = m_EnemyTypeSettings.basicMoveSpeed;

        enemyRandomizer.ActivateRandomSkin();
        healthBarHandler.RefreshHealthBar();
    }
    
    public void OnDespawned()
    {
        enemyManager.RemoveEnemy(this);
        isInGroupRange = false;
    }

    #endregion

    #region Public Methods
    public void RotateTowardsClosestTarget()
    {
        var rot = UtilityFunctions.GetRotationValueTo(ClosestPlayer.GetCurrentTransform(), transform);
        
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, rot, 1);
        model.transform.eulerAngles = new Vector3(0,model.transform.eulerAngles.y, 0);
    }

    public void SlowDownEffect(float slowDownAmt, float slowDownTime)
    {
        
        if (!m_SlowDownActive)
            enemyRandomizer.renderer.material.DOColor(GameSettingsEnemy.slowDownEffect, .1f);
            
        m_SlowDownActive = true;
        m_SlowDownPercentage = slowDownAmt;
        m_SlowDownTimeAmt += slowDownTime;
        

    }
    
    
    
    public bool ReceiveDamage(float damage, Vector3 hitOrigin)
    {
        if (isDead) return true;
        
        healthBarHandler.ActivateHealthBar();

        EffectsManager.Instance.DoZombieHitEffect(transform.position + hitOrigin.normalized, transform);
        
        currentHealth -= damage;
        healthBarHandler.ChangeHealth(currentHealth/maxHealth);

        if (!(currentHealth <= 0)) return false;
        
        ChangeState(EnemyStates.Dead);
        
        return true;

    }

    #endregion
}

public enum EnemyStates
{
    Idle,
    Move,
    Attack,
    Dead
}
