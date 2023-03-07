using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private EnemyManager m_EnemyManager;
    [Space(10)]
    [SerializeField] private GameSettings m_GameSettings;
    [SerializeField] private GameSettingsEnemyPrefabs m_GameSettingsEnemyPrefabs;
    
    [Header("Settings")] 
    [SerializeField] private Vector2 xSpawnPosMinMax;
    [SerializeField] private Vector2 zSpawnPosMinMax;
    [SerializeField] private float m_SpawnCallTick;
    [Space(10)] 
    [SerializeField] private float m_OverlapCheckRadius;
    [Space(10)]
    [SerializeField] private float m_LargeZombieSpawnChance;
    [Space(10)]
    [SerializeField] private LayerMask m_SpawnLayerMask;
    [SerializeField] private LayerMask m_OverlapLayerMask;

    private bool m_DoSpawn;

    private float m_TickRate;
    private float m_NextSpawnTick;

    void Awake()
    {
        m_GameSettings = GameHub.Instance.gameSettings;
        m_GameSettingsEnemyPrefabs = GameHub.Instance.gameSettingsEnemyPrefabs;
    }

    void Update()
    {
        if (!m_DoSpawn) return;
        
        m_TickRate += Time.deltaTime;

        if (!(m_TickRate >= m_NextSpawnTick)) return;
        
        m_NextSpawnTick = m_GameSettingsEnemyPrefabs.GetNextEnemySpawnTick(m_GameSettings.m_GroupTravelledDistance);
        m_TickRate = 0f;
        
        SpawnEnemiesBurst(m_GameSettingsEnemyPrefabs.GetEnemySpawnAmt(m_GameSettings.m_GroupTravelledDistance));
    }

    public void SpawnEnemiesBurst(int numberOfEnemies)
    {

        Ray ray;
        RaycastHit raycastHit;
        
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var position = transform.position + new Vector3(Random.Range(xSpawnPosMinMax.x, xSpawnPosMinMax.y), 5,
                Random.Range(zSpawnPosMinMax.x, zSpawnPosMinMax.y));

            ray = new Ray(position, -Vector3.up);

            if (!Physics.Raycast(ray, out raycastHit, 10, m_SpawnLayerMask)) continue;
            
            var hitEnemies = Physics.OverlapSphere(raycastHit.point, m_OverlapCheckRadius, m_OverlapLayerMask);
            
            if (hitEnemies.Length > 0) continue;

            SpawnEnemy(raycastHit.point + (0.02f * Vector3.up));

        }
    }

    private void SpawnEnemy(Vector3 pos)
    {

        var enemyPrefab = Random.value <= m_LargeZombieSpawnChance
            ? m_GameSettingsEnemyPrefabs.largeEnemy
            : m_GameSettingsEnemyPrefabs.basicEnemy;
        
        var newEnemy = PoolManager.Pools["Enemies"].Spawn(enemyPrefab, pos + (0.3f * Vector3.up), Quaternion.identity, m_EnemyManager.transform);
        var newEnemyScript = newEnemy.GetComponent<Enemy>();

        newEnemyScript.SubscribeToEnemyManager(m_EnemyManager);
        m_EnemyManager.spawnedEnemies.Add(newEnemyScript);
    }
}
