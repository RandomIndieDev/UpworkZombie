using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRepeater : MonoBehaviour
{
    
    public GameObject[] m_GroundPlains;
    [Space(10)]
    public bool m_MoveFirstPlain;
    public float m_ZMoveAmt;
    [Space(10)] 
    public float m_PlayerPassedDistance;

    void OnEnable()
    {
        UIEvents.Instance.OnDistanceTick += CheckIfToSpawnNextPlain;
    }

    void OnDisable()
    {
        UIEvents.Instance.OnDistanceTick -= CheckIfToSpawnNextPlain;
    }


    void CheckIfToSpawnNextPlain(object data)
    {
        var position = (Vector3) data;
        var currentPlainIndex = m_MoveFirstPlain ? 0 : 1;
        
        if ((position.z - m_GroundPlains[currentPlainIndex].transform.position.z) <= m_PlayerPassedDistance) return;
        
        MovePlain(currentPlainIndex);
        m_MoveFirstPlain = !m_MoveFirstPlain;
    }

    void MovePlain(int index)
    {
        m_GroundPlains[index].transform.position += new Vector3(0,0,m_ZMoveAmt * 2);
    }
}
