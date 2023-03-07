using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RunnerRunState : IState
{
    private BaseCharacter owner;

    public RunnerRunState(BaseCharacter owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.AnimationHandler.ResetAnimationValues(owner.WeaponTypeInt);
        owner.AnimationHandler.RunAnimation();
        
        TurnStraight();
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        owner.DoHealEffect();
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

    private void TurnStraight()
    {
        owner.transform.DOLocalRotate(new Vector3(0,0,0), owner.GameSettingsCharacters.rotateTowardsSpeed);
    }
}
