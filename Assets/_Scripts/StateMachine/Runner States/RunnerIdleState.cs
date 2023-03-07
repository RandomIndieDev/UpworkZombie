using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerIdleState : IState
{
    private BaseCharacter owner;

    public RunnerIdleState(BaseCharacter owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.AnimationHandler.ResetAnimationValues(owner.WeaponTypeInt);
        owner.AnimationHandler.StopRun();
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        owner.DoTargetUpdate();
        
        if (CanAttack())
        {
            owner.ChangeState(CharacterStates.Attack);
        }
    }

    public void Exit()
    {
        
    }
    
    public bool CanAttack()
    {
        if (owner.ClosestTarget == null) return false;
        if (owner.ClosestTarget.IsDead()) return false;
        if (!owner.IsInRangeOfTarget()) return false;
        return true;
    }
}
