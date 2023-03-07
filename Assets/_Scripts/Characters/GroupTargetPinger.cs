using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTargetPinger : MonoBehaviour
{
    private GroupManager groupManager;

    private void Awake()
    {
        groupManager = GetComponent<GroupManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("NewSurvivor"))
        {

            var survivorGroup = other.GetComponent<NewSurvivorGroup>();
            groupManager.NewSurvivorsFound(survivorGroup);
        }


        if (!other.CompareTag("Enemy")) return;
        
        var enemy = other.GetComponent<Enemy>();
        
        groupManager.AddTarget(enemy.GetComponent<ICombat>());
        enemy.ChangeState(EnemyStates.Move);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NewSurvivor"))
        {
            groupManager.DeclineNewSurvivors();
        }

        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.DespawnUnit();
        }
    }
}
