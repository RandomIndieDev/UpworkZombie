using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;

public class GameOverStatsPanel : MonoBehaviour
{
    [Header("References")] 
    public UIView m_View;
    public TextMeshProUGUI m_MetersTraveledValueText;
    public TextMeshProUGUI m_KilledValueText;

    public AnimationCurve m_TextShowAnimationCurve;

    private int m_MetersTravelled;
    private int m_ZombiesKilled;

    public void SetupEndingStats(int metersTravelled, int zombiesKilled)
    {
        m_MetersTravelled = metersTravelled;
        m_ZombiesKilled = zombiesKilled;
        
        m_MetersTraveledValueText.text = "0m";
        m_KilledValueText.text = "0";
        
        ShowPanel(1f);
    }

    private IEnumerator ShowStatsOverTime(int metersTravelled, int zombiesKilled)
    {
        for (int i = 0; i <= metersTravelled; i++)
        {
            m_MetersTraveledValueText.text = $"{metersTravelled}m";
            yield return new WaitForSeconds(.5f * m_TextShowAnimationCurve.Evaluate((float) i / metersTravelled));
        }
        
        for (int i = 0; i <= zombiesKilled; i++)
        {
            m_KilledValueText.text = zombiesKilled.ToString();

            float waitTime;
                
            if (zombiesKilled == 0)
            {
                yield break;
            }

            waitTime = .5f * m_TextShowAnimationCurve.Evaluate((float)i / zombiesKilled);

            yield return new WaitForSeconds(waitTime);
        }
    }

    public void ShowPanel(float delayTime)
    {
        Invoke(nameof(ShowPanelAfterDelay), delayTime);
    }

    private void ShowPanelAfterDelay()
    {
        m_View.Show();
        StartCoroutine(ShowStatsOverTime(m_MetersTravelled, m_ZombiesKilled));
    }
}
