using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunCharacter : BaseCharacter
{

    void Awake()
    {
        Init(GameHub.Instance.levelProgressionTracker.GetHandgunStats(), CharacterType.Handgun);
    }
    public override void DoAttackStart()
    {
        animationHandler.StartRangedAttack();
    }

    public override void DoAttack()        
    {
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        var bullet = Instantiate(EffectsManager.Instance.GetHandGunProjectile(), shootLoc.transform.position, shootLoc.transform.rotation);

        if (closestTarget != null)
        {
            var bulletScript = bullet.GetComponent<ProjectileMoveScript>();
            
            bulletScript.SetTarget(closestTarget.GetCurrentTransform().gameObject);
            bulletScript.Damage = currentDamage;
            
            AudioManager.instance.PlayPistolSound();
        }
        else
        {
            Destroy(bullet);
        }
    }
}
