using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsEnvironment", menuName = "Game Settings/Env", order = 1)]
public class GameSettingsEnvironment : ScriptableObject
{


    [Header("Set Pieces")] 
    public List<GameObject> setPieces;
    
    [Header("Environment Set Piece Spawn Settings")] 
    public Vector2 setPieceRotationValues;
    public Vector2 setPieceXPosValues;
}
