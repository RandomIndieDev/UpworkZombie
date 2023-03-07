using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using UnityEngine;
using Lean.Touch;
using Lofelt.NiceVibrations;

public class GroupInteractionHandler : MonoBehaviour
{
    [Header("Game Settings")]
    public GameSettings gameSettings;

    [Header("References")] 
    public Camera mainCamera;

    public LineRenderer m_SwipeLineRenderer;

    public GameObject m_SwipeSphere1;
    public GameObject m_SwipeSphere2;
    
    public LeanPlane m_SwipePlane;
    public GroupPositionSwitcher positionSwitcher;
    public GroupCharacterSelector characterSelector;
    
    private bool hasClicked;
    private bool functionalityEnabled;

    private bool swipeActionCalled;
    
    private bool m_SwipeActionActive;
    private bool m_SplitActionCalled;

    private Vector2 fingerInitialDownPos;

    private GroupPosition initalSelectedPos;

    void Start()
    {
        hasClicked = false;
        functionalityEnabled = false;
        swipeActionCalled = false;

        HideSwipeUI();
    }

    void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerTap += OnFingerTap;
        
        GameHub.Instance.eventsManager.OnCutsceneEventHappened += DisableFunctionality;
        GameHub.Instance.eventsManager.OnCutsceneEventEnded += EnableFunctionality;
        GameHub.Instance.eventsManager.OnGameStarted += EnableFunctionality;
        
        GameHub.Instance.eventsManager.OnSwipeActionStart += EnableSwipeAction;
        GameHub.Instance.eventsManager.OnSwipeActionEnded += ResetSwipeAction;
        
    }

    void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerTap -= OnFingerTap;
        
        GameHub.Instance.eventsManager.OnCutsceneEventHappened -= DisableFunctionality;
        GameHub.Instance.eventsManager.OnCutsceneEventEnded -= EnableFunctionality;
        GameHub.Instance.eventsManager.OnGameStarted -= EnableFunctionality;
        
        GameHub.Instance.eventsManager.OnSwipeActionStart -= EnableSwipeAction;
        GameHub.Instance.eventsManager.OnSwipeActionEnded -= ResetSwipeAction;
    }
    private void OnFingerDown(LeanFinger finger)
    {
        if (!functionalityEnabled) return;

        finger.Age = 0;

        hasClicked = true;
        swipeActionCalled = false;

        initalSelectedPos = GetClickedGroupPosition();
        
        if (initalSelectedPos != null) return;
        if (m_SwipeActionActive) return;
        
        m_SwipeActionActive = true;
        
        var startPos = GetSwipePlanePosition(finger.ScreenPosition);
        
        m_SwipeSphere1.transform.position = startPos;
        
        m_SwipeSphere1.SetActive(true);
        m_SwipeSphere2.SetActive(true);
        
        m_SwipeLineRenderer.enabled = true;
    }

    private void OnFingerSwipe(LeanFinger finger)
    {
        if (!functionalityEnabled) return;
        if (initalSelectedPos != null)
        {
            DetectSwipeDown(finger);
        };
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        if (!hasClicked) return;

        if (m_SwipeActionActive)
        {
            var startPos = GetSwipePlanePosition(finger.StartScreenPosition);
            var endPos = GetSwipePlanePosition(finger.ScreenPosition);

            
            var positions = new List<Vector3>();
            positions.Add(startPos);
            positions.Add(endPos);
            
            m_SwipeSphere2.transform.position = endPos;
            m_SwipeLineRenderer.SetPositions(positions.ToArray());
            
            return;
        }

        var moveDistance = finger.GetScaledDistance(finger.StartScreenPosition);
        
        if (!finger.Old) return;

        if (!IsClickToMoveInput(moveDistance)) return;
        
        hasClicked = false;
        positionSwitcher.DoPositionSelected(GetClickedGroupPosition());

    }

    private void OnFingerUp(LeanFinger finger)
    {
        hasClicked = false;
        
        if (m_SwipeActionActive && !m_SplitActionCalled)
        {
            m_SwipeLineRenderer.enabled = false;
            
            Invoke(nameof(HideSwipeUI), gameSettings.swipeShowDuration);

            if (Vector3.Distance(m_SwipeSphere1.transform.position, m_SwipeSphere2.transform.position) < gameSettings.minSplitDistance)
            {
                DoRedBlink();
                m_SwipeActionActive = false;
                return;
            }

            m_SplitActionCalled = true;
            
            var startPos = GetSwipePlanePosition(finger.StartScreenPosition);
            var endPos = GetSwipePlanePosition(finger.ScreenPosition);
            
            GameHub.Instance.eventsManager.DoOnSwipeAction(new SwipeData(startPos, endPos));
            return;
        };
        
        if (!functionalityEnabled) return;

        finger.Age = 0f;
        positionSwitcher.DoPositionDeSelected(GetClickedGroupPosition());
        
        
    }

    private void OnFingerTap(LeanFinger finger)
    {
        if (!functionalityEnabled) return;
        hasClicked = false;
        
        characterSelector.ShowCharacterDetails(GetClickedGroupPosition());
    }

    #region Player Input Detection

    private bool IsClickToMoveInput(float moveDis)
    {
        if (initalSelectedPos == null) return false;
        if (swipeActionCalled) return false;
        
        return moveDis >= 5f;
    }

    #endregion

    private void EnableSwipeAction(object data)
    {
        m_SwipeActionActive = true;
    }

    private void ResetSwipeAction(object data)
    {
        m_SwipeActionActive = false;
        m_SplitActionCalled = false;
    }

    private void DisableFunctionality()
    {
        functionalityEnabled = false;
    }

    private void EnableFunctionality()
    {
        functionalityEnabled = true;
    }
    
    private GroupPosition GetClickedGroupPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(ray, out var hit, 100f, gameSettings.groupSelectionMask)) return null;
        
        return hit.collider.gameObject.GetComponent<GroupPosition>();
    }

    private void DoRedBlink()
    {
        m_SwipeSphere1.GetComponent<Renderer>().material.color = Color.red;
        m_SwipeSphere2.GetComponent<Renderer>().material.color = Color.red;
    }

    private void HideSwipeUI()
    {        
        m_SwipeSphere1.SetActive(false);
        m_SwipeSphere2.SetActive(false);
        
        m_SwipeSphere1.GetComponent<Renderer>().material.color = Color.white;
        m_SwipeSphere2.GetComponent<Renderer>().material.color = Color.white;
    }

    private Vector3 GetSwipePlanePosition(Vector3 mousePos)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        return m_SwipePlane.GetClosest(ray);
    }
    
    void DetectSwipeDown(LeanFinger finger)
    {
        if (VerticalMoveValue (finger) > HorizontalMoveValue (finger)) {
            if (finger.StartScreenPosition.y - finger.ScreenPosition.y > 0) {
                OnSwipeDown();
            }
        } 
    }

    float VerticalMoveValue (LeanFinger finger)
    {
        return Mathf.Abs (finger.StartScreenPosition.y - finger.ScreenPosition.y);
    }

    float HorizontalMoveValue (LeanFinger finger)
    {
        return Mathf.Abs (finger.StartScreenPosition.x - finger.ScreenPosition.x);
    }
    
    void OnSwipeDown ()
    {
        if (initalSelectedPos == null || swipeActionCalled) return;
        swipeActionCalled = true;

        if (!m_SwipeActionActive)
        {
            positionSwitcher.RemoveAtPosition(initalSelectedPos);
            
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
        }
        else
        {
            positionSwitcher.RemoveAllInGroup(initalSelectedPos);
            
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
            GameHub.Instance.eventsManager.DoOnSwipeActionEnded(null);
        }
    }
    
    public struct SwipeData
    {
        public Vector3 StartLocation;
        public Vector3 EndLocation;

        public SwipeData(Vector3 startLocation, Vector3 endLocation)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
        }
    }
}
