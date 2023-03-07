using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TitleText;

    public AnimationCurve curve;

    private string m_Text = "ZOMBIE RUN!";

    public void Start()
    {
        m_TitleText.text = "";
        StartCoroutine(DoText());
    }

    private IEnumerator DoText()
    {
        var count = 1f;
        foreach (var text in m_Text)
        {
            var waitTime = .20f * curve.Evaluate(count / m_Text.Length);

            yield return new WaitForSeconds(text.CompareTo(' ') == 0 ? 0 : waitTime);
            
            count += 1;
            m_TitleText.text += text;
        }
    }
    
    
}
