using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    private int _lastWidth = 0;
    private int _lastHeight = 0;
    private float _fixTime = 0.5f;
    private float _time = 0.0f;
    private bool _isFixed = false;

    private void Awake()
    {
        var heightAccordingToWidth = Screen.width / 16.0f * 9.0f;
        Screen.SetResolution(Screen.width, Mathf.FloorToInt(heightAccordingToWidth), false, 0);

        var widthAccordingToHeight = Screen.height / 9.0f * 16.0f;
        Screen.SetResolution(Mathf.FloorToInt(widthAccordingToHeight), Screen.height, false, 0);

        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
    }

    private void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (_lastHeight != height || _lastWidth != width)
        {
            _isFixed = false;
            _time = 0;
        }
        else
        {
            if (_time >= _fixTime && !_isFixed)
            {
                var heightAccordingToWidth = width / 16.0f * 9.0f;
                Screen.SetResolution(width, Mathf.FloorToInt(heightAccordingToWidth), false, 0);

                var widthAccordingToHeight = height / 9.0f * 16.0f;
                Screen.SetResolution(Mathf.FloorToInt(widthAccordingToHeight), height, false, 0);

                _isFixed = true;
            }

            _time += Time.deltaTime;
        }
        _lastWidth = width;
        _lastHeight = height;
    }
}
