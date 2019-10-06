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
    }

    #endregion
}
