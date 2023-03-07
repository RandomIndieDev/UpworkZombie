using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] private EnemySpawner m_EnemySpawner;
    [SerializeField] private RunnerSpawner m_RunnerSpawner;

    [Space(10)] 
    [SerializeField] private Vector2 m_SpawnRunnerTickMinMax;
    [SerializeField] private float m_StopEnemySpawnTime;

    private EventsManager m_EventManager;
    
    private GameSettings m_GameSettings;
    private GameSettingsEnemyPrefabs m_GameSettingsEnemyPrefabs;

    private bool m_DoSpawn;
    
    private float m_TickRate;
    private float m_RunnerSpawnTickRate;
    
    private float m_NextEnemySpawnTick;
    private float m_NextRunnerSpawnTick;

    
    private void Awake()
    {
        m_EventManager = GameHub.Instance.eventsManager;
        m_GameSettings = GameHub.Instance.gameSettings;
        m_GameSettingsEnemyPrefabs = GameHub.Instance.gameSettingsEnemyPrefabs;
    }

    private void OnEnable()
    {
        m_EventManager.OnGameStarted += StartSpawning;
        
        m_EventManager.OnSwipeActionStart += StopSpawning;
        m_EventManager.OnSwipeActionEnded += StartSpawning;
        
    }

    void OnDisable()
    {
        m_EventManager.OnGameStarted -= StartSpawning;
        
        m_EventManager.OnSwipeActionStart -= StopSpawning;
        m_EventManager.OnSwipeActionEnded -= StartSpawning;
    }
    
    void Update()
    {
        if (!m_DoSpawn) return;
        
        m_TickRate += Time.deltaTime;
        m_RunnerSpawnTickRate += Time.deltaTime;


        if (m_RunnerSpawnTickRate >= m_NextRunnerSpawnTick - m_StopEnemySpawnTime)
        {
            if (!(m_RunnerSpawnTickRate >= m_NextRunnerSpawnTick)) return;
            
            m_NextRunnerSpawnTick = Random.Range(m_SpawnRunnerTickMinMax.x, m_SpawnRunnerTickMinMax.y);
            m_RunnerSpawnTickRate = 0f;
            
            m_RunnerSpawner.SpawnSurvivors(GameHub.Instance.GetSurvivorSpawnAmt());
            
            return;

        }

        if (m_RunnerSpawnTickRate < m_StopEnemySpawnTime)
        {
            return;
        }
        
        if (!(m_TickRate >= m_NextEnemySpawnTick)) return;
    
        m_NextEnemySpawnTick = m_GameSettingsEnemyPrefabs.GetNextEnemySpawnTick(m_GameSettings.m_GroupTravelledDistance);
        m_TickRate = 0f;
    
        m_EnemySpawner.SpawnEnemiesBurst(m_GameSettingsEnemyPrefabs.GetEnemySpawnAmt(m_GameSettings.m_GroupTravelledDistance));
        
    }
    private void StartSpawning()
    {
        m_DoSpawn = true;
        m_NextEnemySpawnTick = 0;
        m_NextRunnerSpawnTick = Random.Range(m_SpawnRunnerTickMinMax.x, m_SpawnRunnerTickMinMax.y);
    }

    private void StartSpawning(object data)
    {
        StartSpawning();
    }


    private void StopSpawning(object data)
    {
        m_DoSpawn = false;
    }
}
