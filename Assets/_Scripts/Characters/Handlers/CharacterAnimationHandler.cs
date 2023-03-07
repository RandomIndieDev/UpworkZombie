using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    public Animator animator;
    public void ResetAnimationValues(int weaponTypeInt)
    {
        animator.SetInteger("WeaponType_int", weaponTypeInt);
        animator.SetInteger("MeleeType_int", weaponTypeInt == 12 ? 1 : 0);
        animator.SetInteger("Animation_int", 0);
    }

    public void StartMeleeAttack()
    {
        animator.SetInteger("WeaponType_int", 12);
        animator.SetInteger("MeleeType_int", 1);
        animator.SetFloat("Speed_f", 0f);
    }

    public void StopMeleeAttack()
    {
        animator.SetInteger("WeaponType_int", 0);
        animator.SetInteger("MeleeType_int", 1);
    }

    public void StartRangedAttack()
    {
        animator.SetInteger("WeaponType_int", 1);
        animator.SetBool("Shoot_b", true);
        animator.SetBool("Reload_b", false);
    }
    
    public void StartRangedAttackSniper()
    {
        animator.SetFloat("Body_Horizontal_f", .5f);
        animator.SetBool("Shoot_b", true);
        animator.SetBool("Reload_b", false);
    }

    public void StopRangedAttack()
    {
        animator.SetFloat("Body_Horizontal_f", 0f);
        animator.SetInteger("WeaponType_int", 0);
        animator.SetBool("Shoot_b", false);
        animator.SetBool("Reload_b", false);
    }

    public void RunAnimation()
    {
        animator.SetFloat("Speed_f", 1f);
    }

    public void StopRun()
    {
        animator.SetFloat("Speed_f", 0f);
    }

    public void SelectedAnimation()
    {
        animator.SetFloat("Speed_f", 0f);
        animator.SetInteger("Animation_int", 1);
    }

}
