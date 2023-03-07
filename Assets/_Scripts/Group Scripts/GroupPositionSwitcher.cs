using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Common;
using Lofelt.NiceVibrations;
using UnityEngine;

public class GroupPositionSwitcher : MonoBehaviour
{
    [Header("References")]
    public List<GroupPosition> possiblePositions;

    public LayerMask mask;

    public GameObject m_SwipePlane;

    public GameObject silhouetteCharacter;

    private bool hasSelected;
    private GroupPosition selectedPos;

    private bool m_AllowPositionDeselect;

    private List<GroupPosition> m_SplitGroupOne;
    private List<GroupPosition> m_SplitGroupTwo;

    private Vector3 m_GroupSplitDirection;



    private void OnEnable()
    {
        UIEvents.Instance.OnNewSurvivorsFound += ResetSelection;

        GameHub.Instance.eventsManager.OnSwipeAction += SplitGroup;
    }

    private void OnDisable()
    {
        UIEvents.Instance.OnNewSurvivorsFound -= ResetSelection;
        
        GameHub.Instance.eventsManager.OnSwipeAction -= SplitGroup;
    }

    private void SplitGroup(object data)
    {
        var swipeData = (GroupInteractionHandler.SwipeData) data;
        
        SplitGroupBetweenPoints(swipeData);

        if (m_SplitGroupOne.Count <= 0 || m_SplitGroupTwo.Count <= 0)
        {
            GameHub.Instance.eventsManager.DoOnSwipeActionEnded(null);
            return;
        }
        
        GameHub.Instance.eventsManager.DoOnSwipeActionStart(null);

        foreach (var groupOne in m_SplitGroupOne)
        {
            groupOne.character.transform.DOMove(groupOne.character.transform.position + m_GroupSplitDirection * .5f, .6f);
        }

        foreach (var groupTwo in m_SplitGroupTwo)
        {
            groupTwo.character.transform.DOMove(groupTwo.character.transform.position + (m_GroupSplitDirection * -1) * .5f, .6f);
        }
    }

    private void SplitGroupBetweenPoints(GroupInteractionHandler.SwipeData data)
    {
        var startPos = data.StartLocation;
        var endPos = data.EndLocation;
        
        m_SplitGroupOne = new List<GroupPosition>();
        m_SplitGroupTwo = new List<GroupPosition>();

        var end = endPos.normalized - startPos.normalized;

        m_SwipePlane.transform.position = Vector3.Lerp(startPos, endPos, 0.5f);

        m_SwipePlane.transform.parent = transform;

        m_SwipePlane.transform.LookAt(startPos);

        var meshPlane = m_SwipePlane.transform.GetChild(0).gameObject.GetComponent<LeanPlane>();
        
        foreach (var position in possiblePositions)
        {
            if (position.character == null) continue;
            
            if (meshPlane.GetDistance(position.m_LocationTransform.position) <= 1)
            {
                m_SplitGroupOne.Add(position);
            }
            else
            {
                m_SplitGroupTwo.Add(position);
            }
        }

        m_GroupSplitDirection = m_SwipePlane.transform.right.normalized;
    }

    private void ResetSelection(object data)
    {
        if (!hasSelected) return;

        hasSelected = false;
        silhouetteCharacter.SetActive(false);
        ActivateIndicators(false);
    }
    
    
    public void DoPositionSelected(GroupPosition position)
    {
        selectedPos = position;
        
        if (position == null) return;
        
        if (!position.IsBeingUsed()) return;
        
        if (position.character == null)
        {
            selectedPos = null;
            return;
        }

        hasSelected = true;
        selectedPos.MoveCharacterUp(silhouetteCharacter);
        
        ActivateIndicators(true);
    }
    
    public void DoPositionDeSelected(GroupPosition groupPosition)
    {
        
        silhouetteCharacter.SetActive(false);
        
        if (hasSelected)
        {
            var pos = groupPosition;
            if (pos != null)
            {
                hasSelected = false;

                var startSelectedPosCharacter = selectedPos.character;
                var endSelectedPosCharacter = pos.character;
                
                selectedPos.ResetSpot();
                pos.ResetSpot();
                
                selectedPos.AddCharacterToPos(endSelectedPosCharacter);
                pos.AddCharacterToPos(startSelectedPosCharacter);

                AudioManager.instance.Play("Char_Switch_Pos");
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                GameHub.Instance.eventsManager.DoCharacterPositionChanged();

                selectedPos = null;

                ActivateIndicators(false);
            }

            hasSelected = false;
        }
            
        ActivateIndicators(false);
    }

    #region Adding And Removing Survivors
    public void AddToMiddlePosition(BaseCharacter character)
    {
        var position = possiblePositions.Find(i => i.m_LocationIndex == 1);
        position.AddCharacterToPos(character);
    }

    public void AddToFreePosition(BaseCharacter character)
    {
        foreach (var position in possiblePositions.Where(position => position.character == null))
        {
            position.AddCharacterToPos(character);
            return;
        }
    }

    public void RemoveAtPosition(GroupPosition groupPosition)
    {
        if (groupPosition.character == null) return;
        var selectedChar = groupPosition.character;
        if (!groupPosition.character.GetComponent<BaseCharacter>().RemoveFromGroup()) return;
        EffectsManager.Instance.DoLastStandEffect(selectedChar.transform.position, selectedChar.transform);
        groupPosition.ResetSpot();
    }

    public void RemoveAllInGroup(GroupPosition groupPosition)
    {
        var groupToRemove = m_SplitGroupOne.Contains(groupPosition) ? m_SplitGroupOne : m_SplitGroupTwo;
        var groupToReset = m_SplitGroupOne.Contains(groupPosition) ? m_SplitGroupTwo : m_SplitGroupOne;
        
        foreach (var position in groupToRemove)
        {
            
            EffectsManager.Instance.DoLastStandEffect(position.character.transform.position, position.character.transform);
            
            RemoveAtPosition(position);
        }

        foreach (var position in groupToReset)
        {
            position.ResetCharacterToPosition();
        }
    }

    #endregion

    private void ActivateIndicators(bool isActive)
    {
        foreach (var position in possiblePositions)
        {
            position.SetupIndicator(isActive);
        }
    }


}
