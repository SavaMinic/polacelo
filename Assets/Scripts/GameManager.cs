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
        EndGame,
    }

    #endregion

    #region Singleton

    public static GameManager I { get; private set; }

    #endregion

    #region Actions

    public Action OnGameStateChanged;

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

    private GameState state;

    private MainEventTrigger eventTrigger;

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

    public bool IsPlaying => state == GameState.Playing;
    public bool IsCardFalling => state == GameState.CardFalling;

    #endregion

    #region Mono

    private void Awake()
    {
        I = this;
        eventTrigger = FindObjectOfType<MainEventTrigger>();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;


        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
    }

    #endregion

    #region Private

    public void NewGame()
    {
        State = GameState.Playing;
        
        // reset cards
        firstCard.ResetTo(
            new Vector3(0f, 18f, 20f),
            Quaternion.Euler(24f, 80f, 65f)
        );
        secondCard.ResetTo(
            new Vector3(0f, 14.5f, 20f),
            Quaternion.Euler(90f, 0f, 90f)
        );
    }

    public void CardsClashed()
    {
        if (state == GameState.CardFalling)
            return;

        var dragDirection = eventTrigger.NormalizedDragDirection;
        firstCard.AddForce(dragDirection, clashDragForce);
        State = GameState.CardFalling;
    }

    public void ReleaseCard()
    {
        if (state != GameState.Playing)
            return;

        var dragDirection = eventTrigger.NormalizedDragDirection;
        firstCard.AddForce(dragDirection, dragForce);
        firstCard.StartFalling();
        State = GameState.CardFalling;
    }

    #endregion
}
