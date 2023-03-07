using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterPathFollower : MonoBehaviour
{
    private GameSettings gameSettings;
    
    [Header("References")]
    public BasicRoad currentRoad;
    public RoadCreator roadCreator;
    
    private bool allowMove;
    private float dstTravelled;
    private float distanceTick;
    private int m_HeldBackValue;

    private float m_MaxGroupMoveSpeed;
    private float m_MinGroupMoveSpeed;

    private int m_CurrentGroupSizeValue;

    public int CurrentGroupSizeValue
    {
        get => m_CurrentGroupSizeValue;
        set => m_CurrentGroupSizeValue = value;
    }
    public int HeldBackValue
    {
        get => m_HeldBackValue;
        set => m_HeldBackValue = value;
    }

    void Awake()
    {
        gameSettings = GameHub.Instance.gameSettings;
        gameSettings.m_GroupTravelledDistance = 0;
    }

    void Start()
    {
        allowMove = false;
        distanceTick = 0;
        HeldBackValue = 0;
        
        m_MaxGroupMoveSpeed = gameSettings.maxGroupMoveSpeed;
        m_MinGroupMoveSpeed = gameSettings.minGroupMoveSpeed;
        
        
        MoveCharacters();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (!allowMove) return;
        
        MoveCharacters();
        CalculateDistancePing();
    }

    public void StopMovement()
    {
        allowMove = false;
    }

    public void StartMovement()
    {
        allowMove = true;
    }

    private void CalculateDistancePing()
    {
        if (!allowMove) return;

        distanceTick += Time.deltaTime * (GetGroupMoveSpeed() * gameSettings.speedPercentageMultiplier);

        if (!(distanceTick >= gameSettings.secondsToMeter)) return;
        
        UIEvents.Instance.DoDistanceTick(transform.position);

        gameSettings.m_GroupTravelledDistance++;
        
        distanceTick = 0f;
    }

    private void MoveCharacters()
    {

        dstTravelled += GetGroupMoveSpeed()* Time.deltaTime;
        transform.position = currentRoad.pathCreator.path.GetPointAtDistance(dstTravelled, EndOfPathInstruction.Stop);
        transform.rotation = currentRoad.pathCreator.path.GetRotationAtDistance(dstTravelled, EndOfPathInstruction.Stop);


        if (transform.position !=
            currentRoad.pathCreator.path.GetPoint(currentRoad.pathCreator.path.NumPoints - 1)) return;

        currentRoad = roadCreator.GetNewRoadData();
        dstTravelled = 0;
    }

    private float GetGroupMoveSpeed()
    {
        var lerpValue = m_CurrentGroupSizeValue == 1 ? 0 : (m_CurrentGroupSizeValue - 1 / 8f) / 10f;
        
        var speedBasedOnSize = Mathf.Lerp(m_MaxGroupMoveSpeed, m_MinGroupMoveSpeed, lerpValue);

        if (HeldBackValue >= gameSettings.maxGroupHeldBackAmt)
            HeldBackValue = gameSettings.maxGroupHeldBackAmt;

        var heldBackDecreaseAmt = Mathf.Lerp(gameSettings.heldBackSpeedDecrease * gameSettings.heldBackMinSpeedDecreasePercentage, gameSettings.heldBackSpeedDecrease, lerpValue);

        speedBasedOnSize -= (m_HeldBackValue * heldBackDecreaseAmt);

        return speedBasedOnSize;
    }
}
