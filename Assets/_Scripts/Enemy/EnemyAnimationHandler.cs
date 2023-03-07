using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public Animator animator;

    public void DoIdleAnimation()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    public void DoWalkAnimation()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
    }

    public void DoAttack()
    {
        animator.SetBool("IsAttacking", true);
    }
    
}
