using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NewSurvivorGroup : MonoBehaviour
{
    [SerializeField] private List<Transform> m_PossiblePositions;
    
    public List<BaseCharacter> m_NewSurvivors;

    private List<int> m_UsedIndexes;

    public void Awake()
    {
        m_UsedIndexes = new List<int>();
    }

    public void AddSurvivor(BaseCharacter character)
    {

        var keepChecking = true;
        var index = 0;
        
        while (keepChecking)
        {
            index = Random.Range(0, m_PossiblePositions.Count);
            
            if(m_UsedIndexes.Contains(index)) continue;
            
            m_UsedIndexes.Add(index);
            keepChecking = false;
        }
        
        character.AnimationHandler.ResetAnimationValues(character.WeaponTypeInt);
        character.transform.position = m_PossiblePositions[index].position;
        
        m_NewSurvivors.Add(character);
    }
    public void InitiateFound()
    {
        foreach (var survivor in m_NewSurvivors)
        {
            EffectsManager.Instance.DoSurvivorReactionEffect(SurvivorReactions.Shocked, survivor.transform.position, new Vector3(0,4.5f,0), transform);
            survivor.transform.DORotate(new Vector3(0, 180, 0), .3f, RotateMode.WorldAxisAdd);
        }
    }


}
