using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RunnerSpawner : MonoBehaviour
{
    
    [SerializeField] private NewSurvivorGroup m_newSurvivorGroupPrefab;
    
    [SerializeField] private Vector2 xSpawnPosMinMax;
    [SerializeField] private Vector2 zSpawnPosMinMax;
    [SerializeField] private LayerMask m_SpawnLayerMask;
    
    private GameSettingsCharacters m_GameSettingsCharacters;

    public void Awake()
    {
        m_GameSettingsCharacters = GameHub.Instance.gameSettingsCharacters;
    }

    public void SpawnSurvivors(int amt)
    {
        var newSurvivor = GameHub.Instance.GetRandomSurvivor();

        var survivorGroup = Instantiate(m_newSurvivorGroupPrefab, GetRandomSpawnPosition(), Quaternion.identity);

        for (int i = 0; i < amt; i++)
        {
            var survivor = Instantiate(newSurvivor, GetRandomSpawnPosition(), Quaternion.identity).GetComponent<BaseCharacter>();
            survivorGroup.AddSurvivor(survivor);
        } 
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Ray ray;
        RaycastHit raycastHit;
        
        var position = transform.position + new Vector3(Random.Range(xSpawnPosMinMax.x, xSpawnPosMinMax.y), 5,
            Random.Range(zSpawnPosMinMax.x, zSpawnPosMinMax.y));

        ray = new Ray(position, -Vector3.up);
        
        if (!Physics.Raycast(ray, out raycastHit, 10, m_SpawnLayerMask)) return Vector3.zero;

        return raycastHit.point;
    }
    
}
