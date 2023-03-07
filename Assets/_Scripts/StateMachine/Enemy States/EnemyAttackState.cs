using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState
{
    
    private Enemy owner;

    public EnemyAttackState(Enemy owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.IsAttacking = true;
        
        owner.animationHandler.DoIdleAnimation();
        owner.animationHandler.DoAttack();
        owner.healthBarHandler.ActivateHealthBar();
        owner.Rigidbody.velocity = Vector3.zero;
        
        GameHub.Instance.eventsManager.DoEnemyEnterGroupRange(null);
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        if (!IsNearbyTarget())
        {
            owner.StateMachine.ChangeState(new EnemyMoveState(owner));
            return;
        }
        
        owner.RotateTowardsClosestTarget();
        
        if (!CanAttack()) return;
        
        Attack();
    }

    public void Exit()
    {
        owner.IsAttacking = false;
        GameHub.Instance.eventsManager.DoEnemyLeaveGroupRange(null);
        
    }

    private bool CanAttack()
    {
        if (!owner.TriggerAttack) return false;

        owner.TriggerAttack = false;

        return true;
    }

    private void Attack()
    {
        var isTargetDead = false;
        
        if (owner.ClosestPlayer == null) return;

        if (!IsNearbyTarget())
        {
            owner.StateMachine.ChangeState(new EnemyMoveState(owner));
            return;
        }
        
        isTargetDead = owner.ClosestPlayer.ReceiveDamage(owner.CurrentDamage, owner.transform.position);
        
        if (isTargetDead)
        {
            owner.EnemyManager.PingEnemiesToRetarget();
        }
    }
    
    private bool IsNearbyTarget()
    {
        return Vector3.Distance(owner.transform.position, owner.ClosestPlayer.GetCurrentTransform().position) < owner.m_EnemyTypeSettings.attackDistance;

    }
}
