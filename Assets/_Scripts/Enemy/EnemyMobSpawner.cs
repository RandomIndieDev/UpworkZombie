using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobSpawner : MonoBehaviour
{
    [Header("Game Settings")]
    public GameSettingsEnemiesType SettingsEnemiesType;
    
    [Header("Settings")]
    [SerializeField] private float ySpawnPosition;
    [SerializeField] private int spawnCollisionCheckRadius;
    [SerializeField] private LayerMask collisionMask;
    
    private List<Enemy> SpawnEnemiesInZone(Vector3 location, Vector3 xOffset, Vector3 zOffset)
    {
        var spawnedEnemies = new List<Enemy>();

        //var spawnAmt = Random.Range(settingsEnemies.m_EnemySpawnAmtMinMax.x, settingsEnemies.maximumSpawnAmt);
        var spawnAmt = 1;
        for (int i = 0; i < spawnAmt; i++)
        {
            var xPos = Random.Range(xOffset.x, xOffset.y);
            var zPos = Random.Range(zOffset.x, zOffset.y);
            
            var spawnLocation = location + new Vector3(xPos, 0, zPos);
            spawnLocation.y = ySpawnPosition;

            if (Physics.CheckSphere(spawnLocation, spawnCollisionCheckRadius, collisionMask)) continue;

            //var spawnedEnemy = Instantiate(settingsEnemies.standardZombie, spawnLocation, Quaternion.identity);

            //spawnedEnemies.Add(spawnedEnemy.GetComponent<Enemy>());
        }
        
        return spawnedEnemies;
    }
}
