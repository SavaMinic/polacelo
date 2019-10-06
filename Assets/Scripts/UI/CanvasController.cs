using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    #region Singleton

    public static CanvasController I { get; private set; }

    #endregion

    #region Fields

    [SerializeField]
    private CanvasGroup mainMenuGroup;

    [SerializeField]
    private Button playButton;

    #endregion

    #region Mono

    private void Awake()
    {
        I = this;
        GameManager.I.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.I.OnGameStateChanged -= OnGameStateChanged;
    }

    #endregion

    #region Events

    private void OnGameStateChanged()
    {
        switch (GameManager.I.State)
        {
            case GameManager.GameState.MainMenu:
                SetMainMenuActive(true);
                break;
            case GameManager.GameState.Playing:
                SetMainMenuActive(false);
                break;
            case GameManager.GameState.EndGame:
                break;
        }
    }

    #endregion

    #region Private

    private void SetMainMenuActive(bool isActive)
    {
        mainMenuGroup.alpha = isActive ? 1f : 0f;
        mainMenuGroup.interactable = mainMenuGroup.blocksRaycasts = isActive;
    }

    #endregion
}
