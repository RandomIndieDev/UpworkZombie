using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    [Header("Settings")] 
    public GameSettingsEnvironment gameSettingsEnvironment;

    public List<Transform> rightPlaceLocations;
    public List<Transform> leftPlaceLocations;

    private List<GameObject> setPieces;

    private Vector2 rotationValues;
    private Vector2 xPosValues;

    private List<int> selectedIndexes;

    void Awake()
    {
        setPieces = gameSettingsEnvironment.setPieces;

        rotationValues = gameSettingsEnvironment.setPieceRotationValues;
        xPosValues = gameSettingsEnvironment.setPieceXPosValues;
        
        ResetRandomIndexes();
    }

    private int GetRandomNonRepeatingIndex()
    {
        var addedNewIndex = false;

        while (!addedNewIndex)
        {
            var selectedIndex = Random.Range(0, setPieces.Count);
            
            if (selectedIndexes.Contains(selectedIndex)) continue;
            
            selectedIndexes.Add(selectedIndex);
            addedNewIndex = true;
        }

        return selectedIndexes[selectedIndexes.Count - 1];
    }

    private void ResetRandomIndexes()
    {
        selectedIndexes = new List<int>();
    }

    public void RandomizeEnvironment()
    {
        
        foreach (var rightLoc in rightPlaceLocations)
        {
            var spawnLocation = rightLoc.position;
            spawnLocation.x += Random.Range(xPosValues.x, xPosValues.y);
            
            var setPiece = Instantiate(setPieces[GetRandomNonRepeatingIndex()], spawnLocation, Quaternion.identity, transform);
            var randomYRot = Random.Range(rotationValues.x, rotationValues.y);
            setPiece.transform.eulerAngles += new Vector3(0,randomYRot, 0);
        }

        foreach (var leftLoc in leftPlaceLocations)
        {
            var spawnLocation = leftLoc.position;
            spawnLocation.x += Random.Range(xPosValues.x, xPosValues.y);
            
            var setPiece = Instantiate(setPieces[GetRandomNonRepeatingIndex()], spawnLocation, Quaternion.identity, transform);
            
            var rotation = new Vector3(0,180,0);
            rotation.y += Random.Range(-10, 10);
            
            setPiece.transform.eulerAngles = rotation;
        }
        
        ResetRandomIndexes();
    }
}
