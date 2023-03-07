using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCharacter : BaseCharacter
{
    void Awake()
    {
        Init(GameHub.Instance.levelProgressionTracker.GetMeleeStats(), CharacterType.Melee);
    }
    public override void DoAttackStart()
    {
        animationHandler.StartMeleeAttack();
    }

    public override void DoAttack()
    {
        var isEnemyDead = true;
        
        if (closestTarget != null)
        {
            isEnemyDead = closestTarget.ReceiveDamage(currentDamage, transform.position);
        }
        
        AudioManager.instance.Play("Runner_MeleeHit");
        
        
        if (!isEnemyDead) return;
        
        UpdateClosestTarget();
    }
}
