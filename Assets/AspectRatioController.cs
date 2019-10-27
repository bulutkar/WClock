using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    private int _lastWidth = 0;
    private int _lastHeight = 0;

    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (_lastWidth != width) // if the user is changing the width
        {
            // update the height
            var heightAccordingToWidth = width / 16.0f * 9.0f;
            Screen.SetResolution(width, Mathf.FloorToInt(heightAccordingToWidth), false, 0);
        }
        else if (_lastHeight != height) // if the user is changing the height
        {
            // update the width
            var widthAccordingToHeight = height / 9.0f * 16.0f;
            Screen.SetResolution(Mathf.FloorToInt(widthAccordingToHeight), height, false, 0);
        }

        _lastWidth = width;
        _lastHeight = height;
    }
}
