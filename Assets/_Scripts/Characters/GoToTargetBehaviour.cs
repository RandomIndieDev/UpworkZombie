using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTargetBehaviour : MonoBehaviour
{
    public BaseCharacter baseCharacter;

    public GameSettingsEnemiesType GameSettingsEnemiesType;

    private GameObject model;
    
    private bool isMoving;
    private bool isAttacking;
    private bool isDead;

    private void Start()
    {
        model = transform.GetChild(0).gameObject;
    }
    
    private void Update()
    {
        return;
        if (baseCharacter.GetClosestTarget() == null || isDead) return;
        
        if (!isMoving) return;
        
        MoveTowardsClosestTarget();
        RotateTowardsClosestTarget();
    }
    
    
    /*
    public void FindClosestTarget()
    {
        var closestAlly = Mathf.Infinity;

        foreach (var character in characters)
        {
            var dis = Vector3.Distance(transform.position, character.transform.position);
            
            if (!(dis < closestAlly)) continue;
            
            closestPlayer = character.GetComponent<ICombat>();
            closestAlly = dis;
        }

        isMoving = true;
        animationHandler.DoWalkAnimation();
    }*/
    
    private void MoveTowardsClosestTarget()
    {
        Vector3 moveDirection = (baseCharacter.GetClosestTarget().GetCurrentTransform().position - transform.position).normalized;
        moveDirection.y = 0;

        if (Vector3.Distance(transform.position, baseCharacter.GetClosestTarget().GetCurrentTransform().position) < GameSettingsEnemiesType.attackDistance)
        {
            isMoving = false;
            isAttacking = true;
            
            baseCharacter.AnimationHandler.StopRun();
            baseCharacter.AnimationHandler.StartMeleeAttack();
            baseCharacter.HealthBarHandler.ActivateHealthBar();
        }

        transform.Translate(moveDirection * GameSettingsEnemiesType.basicMoveSpeed * Time.deltaTime);
    }
    
    private void RotateTowardsClosestTarget()
    {
        var rot = UtilityFunctions.GetRotationValueTo(baseCharacter.GetClosestTarget().GetCurrentTransform(), transform);
        
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, rot, 1);
        model.transform.eulerAngles = new Vector3(0,model.transform.eulerAngles.y, 0);
    }
}
