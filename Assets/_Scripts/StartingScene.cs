using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StartingScene : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_WindAnimator;

    [SerializeField] private List<GameObject> m_Scenes;


    private EventsManager m_EventManager;
    
    void Awake()
    {
        m_EventManager = GameHub.Instance.eventsManager;
        m_Scenes[Random.Range(0,m_Scenes.Count)].gameObject.SetActive(true);
    }

    void OnEnable()
    {
        m_EventManager.OnGameStarted += DisableStartScene;
    }
    
    void OnDisable()
    {
        m_EventManager.OnGameStarted -= DisableStartScene;
    }

    private void DisableStartScene()
    {
        m_WindAnimator.Stop();
    }
    
    
}
