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
    private CanvasGroup overlayGroup;

    [SerializeField]
    private CanvasGroup playGroup;
    
    [SerializeField]
    private Text playLabel;

    [SerializeField]
    private CanvasGroup endGroup;

    [SerializeField]
    private Image endImageGroup;
    
    [SerializeField]
    private Text endLabel;

    [SerializeField]
    private CanvasGroup bottomGroup;
    
    [SerializeField]
    private Text bottomLabel;

    [SerializeField]
    private Color wonColor;
    
    [SerializeField]
    private Color lostColor;

    [SerializeField]
    private CanvasGroup gameOverGroup;
    
    [SerializeField]
    private CanvasGroup gameOverTapGroup;

    [SerializeField]
    private Color wonBackColor;
    
    [SerializeField]
    private Color lostBackColor;

    #endregion

    #region Mono

    private void Awake()
    {
        I = this;
        GameManager.I.OnGameStateChanged += OnGameStateChanged;
        GameManager.I.OnPointsChanged += OnPointsChanged;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        GameManager.I.OnGameStateChanged -= OnGameStateChanged;
        GameManager.I.OnPointsChanged -= OnPointsChanged;
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
                StartCoroutine(DoNewRoundAnimation());
                RefreshPoints(GameManager.I.Points);
                break;
            case GameManager.GameState.Resolution:
                StartCoroutine(DoResolutionAnimation());
                break;
            case GameManager.GameState.EndGame:
                StartCoroutine(DoEndAnimation());
                break;
        }
    }

    private void OnPointsChanged(int points)
    {
        RefreshPoints(points);
    }

    #endregion

    #region Private

    private IEnumerator DoNewRoundAnimation()
    {
        playLabel.text = GameManager.I.ShouldBeEqual ? "FULL" : "HALF";
        Go.to(playGroup.transform, 0.3f, new GoTweenConfig().scale(1.5f).setEaseType(GoEaseType.SineIn));
        yield return new WaitForSecondsRealtime(0.4f);
        Go.to(playGroup.transform, 0.3f, new GoTweenConfig().scale(1f).setEaseType(GoEaseType.SineOut));
    }
    
    private IEnumerator DoResolutionAnimation()
    {
        var wonTheRound = GameManager.I.WonTheRound;
        endLabel.text = wonTheRound ? "+1 CARDS" : "MISSED!";
        endLabel.color = wonTheRound ? wonColor : lostColor;
        endImageGroup.color = wonTheRound ? wonBackColor : lostBackColor;
        Go.to(endGroup, 0.5f, new GoTweenConfig().floatProp("alpha", 1f).setEaseType(GoEaseType.SineIn));
        yield return new WaitForSecondsRealtime(1f);
        Go.to(endGroup, 0.5f, new GoTweenConfig().floatProp("alpha", 0f).setEaseType(GoEaseType.SineOut));
        
        if (GameManager.I.Points > 0)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            GameManager.I.ResetRound();
        }
        else
        {
            GameManager.I.GameOver();
        }
    }

    private IEnumerator DoEndAnimation()
    {
        gameOverTapGroup.alpha = 0f;
        Go.to(gameOverGroup, 0.3f, new GoTweenConfig().floatProp("alpha", 1f).setEaseType(GoEaseType.SineIn));
        yield return new WaitForSecondsRealtime(0.8f);
        Go.to(gameOverTapGroup, 0.3f, new GoTweenConfig().floatProp("alpha", 1f).setEaseType(GoEaseType.SineIn));
    }

    private void RefreshPoints(int points)
    {
        bottomLabel.text = $"CARDS:{points}";
    }

    private void SetMainMenuActive(bool isActive)
    {
        mainMenuGroup.alpha = isActive ? 1f : 0f;
        mainMenuGroup.interactable = mainMenuGroup.blocksRaycasts = isActive;
        overlayGroup.alpha = isActive ? 0f : 1f;
        if (gameOverGroup.alpha > 0)
        {
            Go.to(gameOverGroup, 0.3f, new GoTweenConfig().floatProp("alpha", 0f).setEaseType(GoEaseType.SineOut));
        }
    }

    #endregion
}
