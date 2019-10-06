using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        State = GameState.Playing;
    }

    #endregion
}
