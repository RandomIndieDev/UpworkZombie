using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("References")] 
    public CinemachineVirtualCamera m_MainCamera;
    public CinemachineVirtualCamera m_StartingCamera;
    
    
    private EventsManager m_EventManager;

    void Awake()
    {
        m_EventManager = GameHub.Instance.eventsManager;
    }

    void OnEnable()
    {
        m_EventManager.OnGameStarted += SwitchToMainCamera;
    }

    void OnDisable()
    {
        m_EventManager.OnGameStarted -= SwitchToMainCamera;
    }

    public void SwitchToMainCamera()
    {
        m_StartingCamera.Priority = 9;
    }
    
    
    
    
}
