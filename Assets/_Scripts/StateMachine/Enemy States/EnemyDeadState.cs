using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class EnemyDeadState : IState
{
    
    private Enemy owner;

    public EnemyDeadState(Enemy owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.IsDead = true;
        owner.healthBarHandler.DeactivateHealthBar();
        
        AudioManager.instance.Play("Zombie_Death");
        DoDeathAnimation();
        
        GameHub.Instance.eventsManager.DoEnemyDied(null);
        PoolManager.Pools["Enemies"].Despawn(owner.transform);
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        owner.IsDead = false;
    }
    
    public void DoDeathAnimation()
    {
        EffectsManager.Instance.DoDeathEffect(DeathEffects.enemy, owner.transform.position, new Vector3(0,1,0));
    }
}
