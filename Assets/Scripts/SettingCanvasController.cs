using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingCanvasController : MonoBehaviour
{
    public static SettingCanvasController Instance;
    [SerializeField] private Canvas settingCanvas;
    [SerializeField] private Canvas defaultSubCanvas;
    [SerializeField] private Canvas wallpaperCanvas;
    [SerializeField] private Toggle dynamicToggle;
    [SerializeField] private ToggleGroup toggleGroup;
    private Canvas _activeCanvas;
    private bool _dynamic;
    private void Awake()
    {
        Instance = this;
        _activeCanvas = defaultSubCanvas;
    }
    public int GetSelectedToggle()
    {
        var selection = toggleGroup.ActiveToggles().FirstOrDefault();
        return Convert.ToInt32(selection.name);
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

    public void SetWallpaperCanvasConditions()
    {
        dynamicToggle.isOn = CanvasController.Instance.isWallpaperDynamic;
        toggleGroup.gameObject.SetActive(!dynamicToggle.isOn);
        toggleGroup.transform.GetChild(CanvasController.Instance.defaultBgIndex).GetComponent<Toggle>().isOn = true;
    }
    public void CloseSettingCanvas()
    {
        CanvasController.Instance.CloseMainCanvas(settingCanvas.gameObject);
        SoundManager.Instance.StopClip();
    }

    public void OnDynamicChange()
    {
        toggleGroup.gameObject.SetActive(!dynamicToggle.isOn);
    }

    public void OnWallpaperSave()
    {
        _dynamic = dynamicToggle.isOn;
        CanvasController.Instance.ChangeDynamicOption(_dynamic);
        if (_dynamic)
        {
            CanvasController.Instance.ChangeWallpaperDynamically();
        }
        else
        {
            var index = GetSelectedToggle();
            CanvasController.Instance.SetDefaultBg(index);
        }
        CloseSettingCanvas();
    }
}
