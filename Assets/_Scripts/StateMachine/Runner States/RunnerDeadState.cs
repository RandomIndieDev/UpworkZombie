using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerDeadState : IState
{
    
    private BaseCharacter owner;

    public RunnerDeadState(BaseCharacter owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        if (!owner.IsAlive) return;
        
        owner.IsAlive = false;
        
        EffectsManager.Instance.DoDeathEffect(DeathEffects.survivor, owner.transform.position, new Vector3(0,1,0));
        
        owner.GroupManager.RemoveSurvivorFromGroup(owner);
        
        owner.transform.parent = null;
        owner.transform.GetChild(0).gameObject.SetActive(false);
        owner.HealthBarHandler.DeactivateHealthBar();
        owner.HealthBarHandler.DeactivateCanvas();
        
        GameHub.Instance.eventsManager.DoCharacterDied();
        UIEvents.Instance.DoPlayerDeath(owner.characterStats);
        
    }

    public void FixedExecute()
    {
        
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
