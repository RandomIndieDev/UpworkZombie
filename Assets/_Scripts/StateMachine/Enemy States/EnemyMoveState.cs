using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : IState
{
    private Enemy owner;

    public EnemyMoveState(Enemy owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.IsMoving = true;
        owner.IsInGroupRange = true;
        AudioManager.instance.PlayZombieNoise();

        owner.animationHandler.DoWalkAnimation();
        
        FindClosestTarget(owner.EnemyManager.GetPossibleTargets());

    }

    public void FixedExecute()
    {
        if (owner.ClosestPlayer == null || owner.IsDead) return;

        MoveToClosestTarget();
        CheckDistanceToClosestTarget();
    }

    public void Execute()
    {
        if (owner.TriggerRetarget)
        {
            FindClosestTarget(owner.EnemyManager.GetPossibleTargets());
            owner.TriggerRetarget = false;
        }
        
        if (owner.ClosestPlayer == null || owner.IsDead) return;
        
        owner.RotateTowardsClosestTarget();
    }

    public void Exit()
    {
        owner.IsMoving = false;
    }

    public void FindClosestTarget(List<BaseCharacter> characters)
    {
        var closestAlly = Mathf.Infinity;

        foreach (var character in characters)
        {
            var dis = Vector3.Distance(owner.transform.position, character.transform.position);
            
            if (!(dis < closestAlly)) continue;
            
            owner.ClosestPlayer = character.GetComponent<ICombat>();
            closestAlly = dis;
        }
    }

    private void MoveToClosestTarget()
    {
        Vector3 moveDirection = (owner.ClosestPlayer.GetCurrentTransform().position - owner.transform.position).normalized;
        moveDirection.y = 0;
        
        owner.rigidbody.velocity = moveDirection * owner.MoveSpeed * Time.fixedDeltaTime * (1f - owner.SlowDownPercentage);
    }
    
    private void CheckDistanceToClosestTarget()
    {
        if (Vector3.Distance(owner.transform.position, owner.ClosestPlayer.GetCurrentTransform().position) < owner.m_EnemyTypeSettings.attackDistance)
        {
            owner.StateMachine.ChangeState(new EnemyAttackState(owner));
        }
    }
    

}
