using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [HideInInspector] public static CanvasController Instance;
    private GameObject _activeCanvas = null;

    public static bool IsMainCanvasOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void AddActiveCanvas(GameObject canvas)
    {
        _activeCanvas = canvas;
    }
    public void CloseActiveCanvas()
    {
        if (_activeCanvas != null) _activeCanvas.SetActive(false);
        _activeCanvas = null;
    }
}
