using System;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using PathologicalGames;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager _instance;

    public static EffectsManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    [Header("Reaction Effects")]
    [SerializeField] private GameObject reactionShockedEffect;
    [SerializeField] private GameObject reactionHappyEffect;
    [SerializeField] private GameObject reactionSadEffect;
    [SerializeField] private GameObject reactionAngryEffect;
    [SerializeField] private GameObject reactionCryEffect;


    [Header("Survivor Powerups")]
    [SerializeField] private GameObject healEffect;
    [Space(5)]
    [SerializeField] private DamageNumber characterLevelUpEffect;
    [SerializeField] public Vector3 levUpEffectOffset;
    [Space(5)] 
    [SerializeField] private DamageNumber characterLastStandEffect;
    [SerializeField] private Vector3 lastStandEffectOffset;
    [Header("Survivor Effects")]
    [SerializeField] private GameObject survivorDeathEffect;

    [SerializeField] private GameObject survivorHitEffect;
    
    [Header("Enemy Effects")]
    [SerializeField] private GameObject exclamationEffect;
    [SerializeField] private GameObject enemyDeathEffect;
    [SerializeField] private GameObject enemyHitEffect;

    [Header("Projectiles")] 
    [SerializeField] private GameObject handgunProjectile;
    [SerializeField] private GameObject sniperProjectile;

    public void DoSurvivorReactionEffect(SurvivorReactions reaction, Vector3 position, Vector3 offset, Transform parent = null)
    {
        GameObject emotion = null;
        
        if (parent == null)
            parent = transform;
        
        switch (reaction)
        {
            case SurvivorReactions.Shocked:
                emotion = reactionShockedEffect;
                break;
            case SurvivorReactions.Angry:
                emotion = reactionAngryEffect;
                break;
            case SurvivorReactions.Happy:
                emotion = reactionHappyEffect;
                break;
            case SurvivorReactions.Sad:
                emotion = reactionSadEffect;
                break;
            case SurvivorReactions.Exclamation:
                emotion = exclamationEffect;
                break;
            case SurvivorReactions.Cry:
                emotion = reactionCryEffect;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null);
        }
        
        Instantiate(emotion, position + offset, Quaternion.identity, parent);
    }

    public void DoDeathEffect(DeathEffects effect, Vector3 position, Vector3 offset, Transform parent = null)
    {
        GameObject deathEffect;
        
        if (parent == null)
            parent = transform;

        switch (effect)
        {
            case DeathEffects.survivor:
                deathEffect = survivorDeathEffect;
                break;
            case DeathEffects.enemy:
                deathEffect = enemyDeathEffect;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
        }
        
        PoolManager.Pools["Effects"].Spawn(deathEffect, position, Quaternion.identity, parent);
    }

    public void DoHealEffect(Vector3 position, Vector3 offset, Transform parent = null)
    {
        if (parent == null)
            parent = transform;
        
        Instantiate(healEffect, position + offset, healEffect.transform.rotation, parent);
    }

    public void DoZombieHitEffect(Vector3 position, Transform parent = null)
    {
        if (parent == null)
            parent = transform;

        PoolManager.Pools["Effects"].Spawn(enemyHitEffect, position, Quaternion.identity, parent);
    }
    
    public void DoSurvivorHitEffect(Vector3 position, Transform parent = null)
    {
        if (parent == null)
            parent = transform;

        PoolManager.Pools["Effects"].Spawn(survivorHitEffect, position, Quaternion.identity, parent);
    }


    public void DoLastStandEffect(Vector3 position, Transform parent = null)
    {
        if (parent == null)
            parent = transform;
        
        var effect = characterLastStandEffect.Spawn(position + lastStandEffectOffset);
        effect.followedTarget = parent;
    }
    public void DoLevelUpEffect(Vector3 position, Transform parent = null)
    {
        if (parent == null)
            parent = transform;
        
        var effect = characterLevelUpEffect.Spawn(position + levUpEffectOffset, 1);
        effect.followedTarget = parent;
    }

    public GameObject GetHandGunProjectile()
    {
        return handgunProjectile;
    }
    
    public GameObject GetSniperProjectile()
    {
        return sniperProjectile;
    }
    
    
}

public enum DeathEffects
{
    survivor,
    enemy
    
}
public enum SurvivorReactions
{
    Shocked,
    Angry,
    Happy,
    Sad,
    Exclamation,
    Cry
}
