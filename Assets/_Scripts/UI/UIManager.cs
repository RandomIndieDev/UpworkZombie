using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Nody.Nodes;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Settings")] 
    public GameSettings gameSettings;
    
    [Header("References")]
    public TextMeshProUGUI distanceText;

    [Header("UI Views")] 
    public UIView gamestartPanelView;
    public UIView dataPanelView;
    public UIView gameoverPanelView;
    

    [Header("Panels")] 
    public CharacterStatsPanel characterStatsPanel;
    public NotificationsPanel notificationsPanel;
    public GameOverStatsPanel gameOverStatsPanel;
    public SurvivorAcceptPanel survivorAcceptancePanel;

    private int distance;

    private bool dataPanelActive;
    private bool gameoverPanelActive;


    void Start()
    {
        distance = 0;
        distanceText.text = "0m";
        
        notificationsPanel.Init(gameSettings);

    }

    void OnEnable()
    {
        UIEvents.Instance.OnDistanceTick += IncrementMeter;
        UIEvents.Instance.OnNewSurvivorsFound += ShowSurvivorAcceptance;
        UIEvents.Instance.OnPlayerDoneSelecting += HideDataPanel;
        UIEvents.Instance.OnShowCharacterStats += ShowCharacterStatsPanel;
        UIEvents.Instance.OnAllPlayersDead += ShowGameoverStatsScreen;

        UIEvents.Instance.OnNewPlayersJoined += ShowJoinedNotification;
        UIEvents.Instance.OnPlayerDeath += ShowPlayerDeath;
        
        UIEvents.Instance.OnPlayerLevelUp += ShowPlayerLevelUp;
        UIEvents.Instance.OnUpdateCharacterStats += UpdateCharacterStatsPanel;
        

    }

    void OnDisable()
    {
        UIEvents.Instance.OnDistanceTick -= IncrementMeter;
        UIEvents.Instance.OnNewSurvivorsFound -= ShowSurvivorAcceptance;
        UIEvents.Instance.OnPlayerDoneSelecting -= HideDataPanel;
        UIEvents.Instance.OnShowCharacterStats -= ShowCharacterStatsPanel;
        UIEvents.Instance.OnAllPlayersDead -= ShowGameoverStatsScreen;
        
        UIEvents.Instance.OnNewPlayersJoined -= ShowJoinedNotification;
        UIEvents.Instance.OnPlayerDeath -= ShowPlayerDeath;
        UIEvents.Instance.OnPlayerLevelUp -= ShowPlayerLevelUp;
        
        UIEvents.Instance.OnPlayerLevelUp -= ShowPlayerLevelUp;

        UIEvents.Instance.OnUpdateCharacterStats -= UpdateCharacterStatsPanel;
    }

    #region NotificationsCall

    private void ShowJoinedNotification(int amt)
    {
        notificationsPanel.AddNotificationToQueue(new NotificationData(amt, null, NotificationType.Joined));
    }
    private void ShowPlayerDeath(CharacterStats attribute)
    {
        notificationsPanel.AddNotificationToQueue(new NotificationData(0, attribute.CharName, NotificationType.Death));
    }
    private void ShowPlayerLevelUp(CharacterStats attribute)
    {
        notificationsPanel.AddNotificationToQueue(new NotificationData(0, attribute.CharName, NotificationType.LevelUp));
    }

    #endregion

    
    private void UpdateCharacterStatsPanel(CharacterStats stats)
    {
        if (!dataPanelView.IsVisible) return;
        if (characterStatsPanel == null) return;
        if (stats == null) return;

        if (!characterStatsPanel.GetCurrentCharName().Equals(stats.CharName)) return;

        characterStatsPanel.SetupStats(stats);
    }
    private void ShowCharacterStatsPanel(CharacterStats stats)
    {
        DeactivateDataPanelItems();
        
        characterStatsPanel.ActivatePanel();
        characterStatsPanel.SetupStats(stats);
        dataPanelView.Show();
    }

    public void HideDataPanel()
    {
        dataPanelView.Hide();
    }

    public void GameMenuPanelClicked()
    {
        if (gameoverPanelActive)
        {
            gameoverPanelView.Hide();
            gameoverPanelActive = false;
        }
        else
        {
            gameoverPanelView.Show();
            gameoverPanelActive = true;
        }
    }

    public void StartGameClicked()
    {
        gamestartPanelView.Hide();
        GameHub.Instance.eventsManager.DoGameStarted();
    }

    public void ResetLevelClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGameClicked()
    {
        Application.Quit();
    }

    private void IncrementMeter(object data)
    {
        distance += 1;
        distanceText.text = distance + "m";
    }

    private void ShowGameoverStatsScreen(object data)
    {
        var stats = (LevelEndStats) data;
        
        gameOverStatsPanel.SetupEndingStats(stats.distanceTravelled, stats.enemiesKilled);
        gameOverStatsPanel.ShowPanel(2);
    }

    private void ShowSurvivorAcceptance(object data)
    {
        var newSurvivorCount = (int)data;
        
        DeactivateDataPanelItems();
        
        survivorAcceptancePanel.gameObject.SetActive(true);

        var enableAccept = GameHub.Instance.m_GroupManager.CanAcceptCharacters();
        
        survivorAcceptancePanel.Init(newSurvivorCount, enableAccept);

        dataPanelView.Show();
    }

    private void DeactivateDataPanelItems()
    {
        dataPanelView.Hide();
        
        survivorAcceptancePanel.gameObject.SetActive(false);
        
        characterStatsPanel.DeactivatePanel();
    }
}
