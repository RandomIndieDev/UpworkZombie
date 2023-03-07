using System.Collections;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RoadCreator : MonoBehaviour
{
    [Header("Settings")]
    public GameSettings gameSettings;
    
    public List<BasicRoad> spawnedRoads;
    
    public BasicRoad RoadPath;
    public EnvironmentCreator environmentCreator;
    
    

    private void Start()
    {
        SpawnRoadSegment();
    }

    private void SpawnRoadSegment()
    {
        var prevPath = spawnedRoads[spawnedRoads.Count - 1].pathCreator;
        var points = prevPath.bezierPath.GetPointsInSegment(prevPath.bezierPath.NumSegments - 1);

        var loc = prevPath.transform.TransformPoint(points[points.Length - 1]);

        var newRoad = Instantiate(RoadPath, loc, Quaternion.identity, transform);

        newRoad.createdEnvironment = Instantiate(environmentCreator, loc, Quaternion.identity, transform);
        newRoad.createdEnvironment.RandomizeEnvironment();
        newRoad.roadMeshCreator.TriggerUpdate();

        spawnedRoads.Add(newRoad);
        
        
        var removeRoad = spawnedRoads[0];
        spawnedRoads.RemoveAt(0);
        Destroy(removeRoad);
        
        GameHub.Instance.eventsManager.DoNewSectionSpawned();
    }

    public BasicRoad GetNewRoadData()
    {
        var road = spawnedRoads[spawnedRoads.Count - 1];
        SpawnRoadSegment();
                
        return road;
    }

    public Vector3 GetRandomPointForSurvivor()
    {
        var lastPath = spawnedRoads[spawnedRoads.Count - 1].pathCreator;
        var randomPoint = (int) Random.Range(gameSettings.spawnAboveThreshold * lastPath.bezierPath.NumPoints, gameSettings.spawnBelowThreshold * lastPath.bezierPath.NumPoints);

        return lastPath.transform.TransformPoint(lastPath.bezierPath.GetPoint(randomPoint));

    }

    public Vector3 GetRandomPointForEnemies()
    {
        var lastPath = spawnedRoads[spawnedRoads.Count - 1].pathCreator;
        var randomPoint = (int) Random.Range(gameSettings.spawnAboveThreshold * lastPath.bezierPath.NumPoints, lastPath.bezierPath.NumPoints);

        return lastPath.transform.TransformPoint(lastPath.bezierPath.GetPoint(randomPoint));
    }
}
