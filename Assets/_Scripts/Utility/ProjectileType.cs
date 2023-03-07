using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileType : MonoBehaviour
{
    public bool hasSlowDownEffect;
    public float m_SlowDownPercentage;
    public float m_SlowDownTimeAmt;

    void Start()
    {
        m_SlowDownPercentage = GameHub.Instance.gameSettingsCharacters.slowDownAmtPercentage;
        m_SlowDownTimeAmt = GameHub.Instance.gameSettingsCharacters.slowDownTime;
    }

}
