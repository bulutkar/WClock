using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;
    [SerializeField] public List<Sprite> wallpapers;
    [SerializeField] private Image background;
    private Sprite _defaultBg;
    private GameObject _activeCanvas = null;
    private SaveDataContainer saveDataContainer;
    public static bool IsMainCanvasOpen;
    public bool isWallpaperDynamic;
    public int defaultBgIndex;

    private string _savePath;
    private void Awake()
    {
        Instance = this;
        _savePath = Application.persistentDataPath + "/wallpaper.dat";
        saveDataContainer = new SaveDataContainer();
        LoadData();
    }

    public void SetWallpaper()
    {
        if (defaultBgIndex >= wallpapers.Count) SetDefaultBg(0);
        else _defaultBg = wallpapers[defaultBgIndex];

        SetWallpaperInitially();
    }
    public void AddWallpaper(Sprite image)
    {
        wallpapers.Add(image);
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
    public void CloseMainCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
        IsMainCanvasOpen = false;
    }
    private void SetWallpaperInitially()
    {
        if (isWallpaperDynamic)
        {
            ChangeWallpaperDynamically();
        }
        else
        {
            SetWallpaperAsDefault();
        }
    }
    public void ChangeWallpaperDynamically()
    {
        //Some weather api things;
    }
    public void SetDefaultBg(int index)
    {
        defaultBgIndex = index;
        saveDataContainer.defaultBgIndex = defaultBgIndex;
        _defaultBg = wallpapers[defaultBgIndex];
        SetWallpaperAsDefault();
        SaveData();
    }
    public void SetWallpaperAsDefault()
    {
        background.sprite = _defaultBg;
    }
    public void ChangeDynamicOption(bool dynamic)
    {
        isWallpaperDynamic = dynamic;
        saveDataContainer.isWallpaperDynamic = isWallpaperDynamic;
        SaveData();
    }
    private void SaveData()
    {
        FileStream fs = new FileStream(_savePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, saveDataContainer);
        fs.Close();
    }
    private void LoadData()
    {
        if (!File.Exists(_savePath))
        {
            saveDataContainer.defaultBgIndex = defaultBgIndex;
            saveDataContainer.isWallpaperDynamic = isWallpaperDynamic;
            return;
        }
        using (Stream stream = File.Open(_savePath, FileMode.Open))
        {
            var bf = new BinaryFormatter();

            saveDataContainer = (SaveDataContainer)bf.Deserialize(stream);
            isWallpaperDynamic = saveDataContainer.isWallpaperDynamic;
            defaultBgIndex = saveDataContainer.defaultBgIndex;
        }
    }
    [Serializable]
    class SaveDataContainer
    {
        public bool isWallpaperDynamic;
        public int defaultBgIndex;
    }
}
