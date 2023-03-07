using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState
{
    private Enemy owner;

    public EnemyIdleState(Enemy owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.ClosestPlayer = null;
        owner.IsMoving = false;
        owner.IsAttacking = false;
        owner.animationHandler.DoIdleAnimation();
        owner.Rigidbody.velocity = Vector3.zero;
        
    }

    public void FixedExecute()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {
        DoExclamationEffect();
    }
    
    private void DoExclamationEffect()
    {
        EffectsManager.Instance.DoSurvivorReactionEffect(SurvivorReactions.Exclamation, owner.transform.position, new Vector3(0,3,0), owner.transform);
    }
}
