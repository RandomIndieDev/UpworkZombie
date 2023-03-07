using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationBridge : MonoBehaviour
{
    public Enemy enemy;

    public void DoAttack()
    {
        enemy.DoTriggerAttack();
        AudioManager.instance.Play("Zombie_Melee");
    }
}
