using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    public Enemy enemy;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Projectile")) return;
        var projectileEffect = collision.transform.GetComponent<ProjectileMoveScript>();
        
        var damage = projectileEffect.Damage;

        if (projectileEffect.m_ProjectileType.hasSlowDownEffect)
        {
            enemy.SlowDownEffect(projectileEffect.m_ProjectileType.m_SlowDownPercentage, projectileEffect.m_ProjectileType.m_SlowDownTimeAmt);
        }

        enemy.ReceiveDamage(damage, collision.contacts[0].point);
    }
}
