using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBarHandler : MonoBehaviour
{
    [Header("References")] 
    public Slider slider;
    public Canvas m_Canvas;
    
    private GameSettings gameSettings;
    
    void Start()
    {
        RefreshHealthBar();
    }

    public void Init(GameSettings settings)
    {
        gameSettings = settings;
    }

    public void RefreshHealthBar()
    {
        slider.value = 1;
    }

    public void DeactivateHealthBar()
    {
        slider.gameObject.SetActive(false);
    }
    
    public void ActivateHealthBar()
    {
        slider.gameObject.SetActive(true);
    }

    public void DeactivateCanvas()
    {
        m_Canvas.gameObject.SetActive(false);
    }

    public void ChangeHealth(float value)
    {
        if (value <= 0)
            value = 0;
        slider.DOValue(value, gameSettings.healthValueToSpeed);
    }
}
