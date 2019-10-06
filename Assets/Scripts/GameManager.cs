using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Definition

    public enum GameState
    {
        MainMenu,
        Playing,
        CardFalling,
        WaitingForEnd,
        Resolution,
        EndGame,
    }

    private const string HighScoreKey = "HighScore";

    #endregion

    #region Singleton

    public static GameManager I { get; private set; }

    #endregion

    #region Actions

    public Action OnGameStateChanged;
    public Action<int> OnPointsChanged;

    #endregion

    #region Fields

    [SerializeField]
    private Card firstCard;

    [SerializeField]
    private Card secondCard;

    [SerializeField]
    private float dragForce;
    
    [SerializeField]
    private float clashDragForce;

    [SerializeField]
    private AnimationCurve cardYRotation;

    private GameState state;
    private int points;

    private MainEventTrigger eventTrigger;

    private int cardsReachedGround = 0;

    #endregion

    #region Properties

    public GameState State
    {
        get => state;
        set
        {
            state = value;
            OnGameStateChanged?.Invoke();
        }
    }

    public bool IsPlaying => State == GameState.Playing;
    public bool IsMainMenu => State == GameState.MainMenu;
    public bool IsEndGame => State == GameState.EndGame;
    
    public bool ShouldBeEqual { get; private set; }
    public bool WonTheRound { get; private set; }

    public int Points
    {
        get => points;
        set
        {
            OnPointsChanged?.Invoke(value);
            points = value;
            if (points > HighScore)
            {
                PlayerPrefs.SetInt(HighScoreKey, points);
            }
        }
    }

    public int HighScore => PlayerPrefs.GetInt(HighScoreKey, 0);

    #endregion

    #region Mono

    private void Awake()
    {
        I = this;
        eventTrigger = FindObjectOfType<MainEventTrigger>();
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        if (State == GameState.WaitingForEnd)
        {
            // check for both cards
            if (firstCard.IsOnTheGround(out var firstFaceUp)
                && secondCard.IsOnTheGround(out var secondFaceUp)
)
            {
                var isEqual = firstFaceUp == secondFaceUp;
                EndRound(isEqual == ShouldBeEqual);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRound();
        }
    }

    #endregion

    #region Public

    public void EndRound(bool isWon)
    {
        WonTheRound = isWon;
        // update points
        if (isWon)
        {
            Points++;
        }
        else
        {
            Points--;
        }
        // check if there is no cards left
        State = GameState.Resolution;
    }

    public void ResetRound()
    {
        ResetCards();
        State = GameState.Playing;
    }

    public void NewGame()
    {
        points = 1;
        ResetRound();
    }

    public void GameOver()
    {
        State = GameState.EndGame;
    }

    public void CardsClashed()
    {
        if (State == GameState.CardFalling)
            return;

        var dragDirection = eventTrigger.NormalizedDragDirection;
        firstCard.AddForce(dragDirection, clashDragForce);
        State = GameState.CardFalling;
    }

    public void ReleaseCard()
    {
        if (State != GameState.Playing)
            return;

        var dragDirection = eventTrigger.NormalizedDragDirection;
        firstCard.AddForce(dragDirection, dragForce);
        firstCard.StartFalling();
        State = GameState.CardFalling;
    }

    public void ReachedGround(Card card)
    {
        cardsReachedGround++;
        if (cardsReachedGround == 2)
        {
            // both cards reached ground, wait for end
            State = GameState.WaitingForEnd;
        }
        else if (!secondCard.IsFalling)
        {
            // one card reached ground, but second is still not flying
            EndRound(false);
        }
    }
    
    public void ShowMainMenu()
    {
        ResetCards();
        State = GameState.MainMenu;
    }

    #endregion

    #region Private

    private void ResetCards()
    {
        cardsReachedGround = 0;
        ShouldBeEqual = Random.value <= 0.5f;
        // reset cards
        firstCard.ResetTo(
            new Vector3(0f, 18f, 20f),
            Quaternion.Euler(24f, cardYRotation.Evaluate(Random.value), 65f)
        );
        secondCard.ResetTo(
            new Vector3(0f, 14.5f, 20f),
            Quaternion.Euler(90f, 0f, 90f)
        );
    }

    #endregion
}
