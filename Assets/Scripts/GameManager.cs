﻿using System;
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
    public bool IsCardFalling => State == GameState.CardFalling;

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

        if (State == GameState.WaitingForEnd)
        {
            // check for both cards
            if (firstCard.IsOnTheGround(out var firstFaceUp)
                && secondCard.IsOnTheGround(out var secondFaceUp)
)
            {
                Debug.LogError($"{firstFaceUp} {secondFaceUp}");
                State = GameState.EndGame;
            }
            
        }

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
        cardsReachedGround = 0;
        
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
            Debug.LogError("FOUL!");
        }
    }

    #endregion
}
