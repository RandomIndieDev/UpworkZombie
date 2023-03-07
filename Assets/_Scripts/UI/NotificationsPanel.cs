using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Progress;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class NotificationsPanel : MonoBehaviour
{
    private GameSettings gameSettings;

    [Header("UI Panel")] 
    public UIView notificationsPanel;
    
    [Header("References")] 
    public GameObject deathPanel;
    public TextMeshProUGUI deathPanelText;
    public GameObject joinedPanel;
    public TextMeshProUGUI joinedPanelText;
    public GameObject levelUpPanel;
    public TextMeshProUGUI levelUpPanelText;
    
    private bool dataPanelRunning;
    private IEnumerator autoHideDataPanel;

    private Queue<NotificationData> m_NotificationQueue;
    private List<string> m_NotificationIgnoreList;
    
    public void Init(GameSettings settings)
    {
        gameSettings = settings;
        m_NotificationQueue = new Queue<NotificationData>();
        m_NotificationIgnoreList = new List<string>();
    }
    
    void OnEnable()
    {
        UIEvents.Instance.OnPlayerDeath += AddCharacterToIgnoreList;
    }

    void OnDisable()
    {
        UIEvents.Instance.OnPlayerDeath -= AddCharacterToIgnoreList;
    }


    private void AddCharacterToIgnoreList(CharacterStats stats)
    {
        m_NotificationIgnoreList.Add(stats.CharName);
    }

    public void SetupDeathPanel(string charName)
    {
        deathPanel.SetActive(true);
        deathPanelText.text = charName + " Died!";
    }

    public void SetupJoinedPanel(int joinedAmt)
    {
        joinedPanel.SetActive(true);
        joinedPanelText.text = joinedAmt + " Joined!";
    }
    
    public void SetupLevelUpPanel(string charName)
    {
        levelUpPanel.SetActive(true);
        levelUpPanelText.text = charName + " LevelUp!";
        
    }

    public void AddNotificationToQueue(NotificationData data)
    {
        m_NotificationQueue.Enqueue(data);
        

        if (!notificationsPanel.IsHidden || m_NotificationQueue.Count > 1) return;

        LoadNextNotification();
    }

    private void DeactivateAllNotifications()
    {
        levelUpPanel.SetActive(false);
        joinedPanel.SetActive(false);
        deathPanel.SetActive(false);
    }


    private void LoadNextNotification()
    {
        var notificationData = new NotificationData();
        var hasNotification = false;

        while (!hasNotification)
        {
            if (m_NotificationQueue.Count <= 0)
            {
                m_NotificationIgnoreList = new List<string>();
                return;
            }
            
            notificationData = m_NotificationQueue.Dequeue();

            if (m_NotificationIgnoreList.Count <= 0) break;
            if (m_NotificationIgnoreList.Contains(notificationData.charName) && notificationData.notificationType != NotificationType.Death) continue;

            hasNotification = true;
        }

        DeactivateAllNotifications();

        switch (notificationData.notificationType)
        {
            case NotificationType.LevelUp:
                SetupLevelUpPanel(notificationData.charName);
                break;
            case NotificationType.Joined:
                SetupJoinedPanel(notificationData.joinedAmt);
                break;
            case NotificationType.Death:
                SetupDeathPanel(notificationData.charName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        notificationsPanel.Show();
    }

}

public struct NotificationData
{
    public int joinedAmt;
    public string charName;
    public NotificationType notificationType;

    public NotificationData(int joinedAmt, string charName, NotificationType notificationType)
    {
        this.joinedAmt = joinedAmt;
        this.charName = charName;
        this.notificationType = notificationType;
    }
}
    
public enum NotificationType
{
    LevelUp,
    Joined,
    Death
}
