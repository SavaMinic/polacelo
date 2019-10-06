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

    private GameState state;

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

    #endregion

    #region Mono

    private void Awake()
    {
        I = this;
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
        firstCard.ResetTo(new Vector3(
            Random.Range(-2f, 2f),
            Random.Range(17f, 18f),
            20f
        ), Quaternion.Euler(
            Random.Range(-45f, 45f),
            Random.Range(-90f, 90f),
            Random.Range(-45f, -45f)
        ));
    }

    #endregion
}
