using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RunnerAttackState : IState
{
    private BaseCharacter owner;

    public RunnerAttackState(BaseCharacter owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.DoAttackStart();
        owner.IsAttacking = true;
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        owner.DoTargetUpdate();
        
        if (!CheckIfCanAttack())
            return;
        
        TurnTowardsEnemy();
        
        if (!CanAttack()) return;
        
        owner.DoAttack();
    }

    public void Exit()
    {
        
    }
    
    private bool CanAttack()
    {
        if (!owner.TriggerAttack) return false;

        owner.TriggerAttack = false;

        return true;
    }

    private bool CheckIfCanAttack()
    {
        if (owner.ClosestTarget == null)
        {
            owner.ChangeState(CharacterStates.Run);
            return false;
        };

        if (owner.ClosestTarget.IsDead())
        {
            owner.ChangeState(CharacterStates.Run);
            return false;
        };

        if (!owner.IsInRangeOfTarget())
        {
            owner.ChangeState(CharacterStates.Run);
            return false;
        };

        return true;
    }
    
    private void TurnTowardsEnemy()
    {
        var rot = UtilityFunctions.GetRotationValueTo(owner.ClosestTarget.GetCurrentTransform(), owner.transform);
        owner.transform.DORotateQuaternion(rot, owner.GameSettingsCharacters.rotateTowardsSpeed);
    }
}
