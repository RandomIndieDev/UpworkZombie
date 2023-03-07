using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHub : MonoBehaviour
{
    private static GameHub _instance;
    public static GameHub Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        
        characterAttributeGenerator.Init(gameSettingsCharacters);
        levelProgressionTracker.Init(gameSettings, gameSettingsCharacters, gameSettingsEnemyPrefabs, gameSettingsLevelProgression);
    }

    [Header("Settings")] 
    public GameSettings gameSettings;
    public GameSettingsCharacters gameSettingsCharacters;
    public GameSettingsEnemyPrefabs gameSettingsEnemyPrefabs;
    public GameSettingsLevelProgression gameSettingsLevelProgression;

    [Header("Utilities")] 
    public CharacterNameGenerator characterNameGenerator;
    public CharacterAttributeGenerator characterAttributeGenerator;
    public LevelProgressionTracker levelProgressionTracker;

    [Header("Runtime Scripts")] 
    public GroupManager m_GroupManager;

    [Header("Events Manager")] 
    public EventsManager eventsManager;
    public GameObject GetRandomSurvivor()
    {
        var randomValue = Random.Range(0, 3);

        return randomValue switch
        {
            0 => gameSettingsCharacters.meleeSurvivor,
            1 => gameSettingsCharacters.handgunSurvivor,
            2 => gameSettingsCharacters.sniperSurvivor,
            _ => gameSettingsCharacters.meleeSurvivor
        };
    }

    public int GetSurvivorSpawnAmt()
    {
        var value = Random.value;

        if (value <= gameSettingsCharacters.survivor1SpawnChance)
        {
            return 1;
        }
        
        if (value <= gameSettingsCharacters.survivor2SpawnChance)
        {
            return 2;
        }

        return 3;
    }


}
