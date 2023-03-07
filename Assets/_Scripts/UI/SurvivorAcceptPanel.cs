using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurvivorAcceptPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_SurvivorCountText;
    [SerializeField] private TextMeshProUGUI m_AcceptButtonText;

    [SerializeField] private Button m_AcceptButton;
    

    public void Init(int survivorCount, bool canAccept = true)
    {
        m_SurvivorCountText.text = survivorCount + " New Survivor!";
        
        if (canAccept)
            EnableAcceptButton();
        else
            DisableAcceptButton();
    }

    private void EnableAcceptButton()
    {
        m_AcceptButton.interactable = true;
        m_AcceptButtonText.text = "Accept";

    }
    
    private void DisableAcceptButton()
    {
        m_AcceptButton.interactable = false;
        m_AcceptButtonText.text = "FULL";
    }
    
}
