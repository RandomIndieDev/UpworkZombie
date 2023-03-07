using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperCharacter : BaseCharacter
{
    void Awake()
    {
        Init(GameHub.Instance.levelProgressionTracker.GetSniperStats(), CharacterType.Sniper);
    }
    public override void DoAttackStart()
    {
        animationHandler.StartRangedAttackSniper();
    }

    public override void DoAttack()        
    {
        SpawnBullet();
    }
    
    private void SpawnBullet()
    {
        var bullet = Instantiate(EffectsManager.Instance.GetSniperProjectile(), shootLoc.transform.position, shootLoc.transform.rotation);

        if (closestTarget != null)
        {
            var bulletScript = bullet.GetComponent<ProjectileMoveScript>();
            bulletScript.SetTarget(closestTarget.GetCurrentTransform().gameObject);
            bulletScript.Damage = currentDamage;
            
            AudioManager.instance.Play("Sniper_Shoot");
            
        }
        else
        {
            Destroy(bullet);
        }
    }
}
