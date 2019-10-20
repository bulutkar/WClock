using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvasController : MonoBehaviour
{
    public static SettingCanvasController Instance;
    [SerializeField] private Canvas settingCanvas;
    [SerializeField] private Canvas defaultSubCanvas;

    private Canvas _activeCanvas;

    private void Awake()
    {
        Instance = this;
        _activeCanvas = defaultSubCanvas;
    }

    public void OpenSettingCanvas()
    {
        _activeCanvas.gameObject.SetActive(false);
        CanvasController.Instance.CloseActiveCanvas();
        CanvasController.IsMainCanvasOpen = true;
        defaultSubCanvas.gameObject.SetActive(true);
        _activeCanvas = defaultSubCanvas;
        settingCanvas.gameObject.SetActive(true);
    }

    public void OpenSubCanvas(Canvas canvas)
    {
        _activeCanvas.gameObject.SetActive(false);
        _activeCanvas = canvas;
        _activeCanvas.gameObject.SetActive(true);
    }

    public void CloseSettingCanvas()
    {
        CanvasController.Instance.CloseMainCanvas(settingCanvas.gameObject);
        SoundManager.Instance.StopClip();
    }
}
