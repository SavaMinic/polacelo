using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private CinemachineVirtualCamera defaultCamera;

    [SerializeField]
    private CinemachineVirtualCamera cardGroupCamera;

    private CinemachineVirtualCamera currentCamera;

    #endregion

    #region Mono

    private void Awake()
    {
        SetCurrentCamera(defaultCamera);
    }

    private void Start()
    {
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
                SetCurrentCamera(defaultCamera);
                break;
            case GameManager.GameState.Playing:
                SetCurrentCamera(defaultCamera);
                break;
            case GameManager.GameState.CardFalling:
                SetCurrentCamera(cardGroupCamera);
                break;
            case GameManager.GameState.EndGame:
                // do nothing for now
                break;
        }
    }

    #endregion

    #region Private

    private void SetCurrentCamera(CinemachineVirtualCamera cam)
    {
        if (currentCamera == cam)
            return;

        defaultCamera.Priority = cam == defaultCamera ? 100 : 0;
        cardGroupCamera.Priority = cam == cardGroupCamera ? 100 : 0;

        currentCamera = cam;
    }

    #endregion
}
