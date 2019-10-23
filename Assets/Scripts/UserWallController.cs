using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserWallController : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private Transform parentContent;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TMP_InputField imagePath;
    private List<string> _imagePaths = new List<string>();

    private List<Sprite> _userWallpapers = new List<Sprite>();

    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/UserWallpaperPaths.dat";
        LoadData();
    }

    private void Start()
    {
        if (_userWallpapers.Count > 0)
        {
            LoadWallpapers();
        }

        CanvasController.Instance.SetWallpaper();
        ShowWallpapers();
    }

    public static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f)
    {
        Texture2D spriteTexture = LoadTexture(filePath);
        if (spriteTexture == null) return null;
        Sprite sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

        return sprite;
    }
    public static Texture2D LoadTexture(string filePath)
    {
        Texture2D tex2D;
        byte[] fileData;

        if (!File.Exists(filePath)) return null;
        fileData = File.ReadAllBytes(filePath);
        var format = ImageChecker.GetImageFormat(fileData);
        if (format == ImageChecker.ImageFormat.Unknown) return null;
        tex2D = new Texture2D(2, 2);
        return tex2D.LoadImage(fileData) ? tex2D : null;
    }

    public void ShowWallpapers()
    {
        for (int i = 0; i < parentContent.childCount; i++)
        {
            var obj = parentContent.GetChild(i).gameObject;
            Destroy(obj);
        }

        for (int i = 0; i < CanvasController.Instance.wallpapers.Count; i++)
        {
            var everyIndex = i;
            var rect = parentContent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 100);
            GameObject contentClone = Instantiate(content, parentContent);
            contentClone.name = i.ToString();

            Vector3 contentPos = contentClone.transform.localPosition;
            contentPos.y -= i * 220;

            contentClone.transform.localPosition = contentPos;

            contentClone.GetComponent<Toggle>().group = toggleGroup;
            contentClone.transform.GetChild(0).GetComponent<Image>().sprite = CanvasController.Instance.wallpapers[i];
            contentClone.SetActive(true);

        }
    }
    private void LoadWallpapers()
    {
        foreach (var image in _userWallpapers)
        {
            CanvasController.Instance.AddWallpaper(image);
        }
    }

    public void OnAdd()
    {
        var path = imagePath.text;
        var sprite = LoadNewSprite(path);
        if (sprite != null)
        {
            _imagePaths.Add(path);
            _userWallpapers.Add(sprite);
            SaveData();
            CanvasController.Instance.wallpapers.Add(sprite);
            ShowWallpapers();
        }

        imagePath.text = "";
    }
    private void SaveData()
    {
        FileStream fs = new FileStream(_savePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, _imagePaths);
        fs.Close();
    }
    private void LoadData()
    {
        List<string> items = new List<string>();
        if (!File.Exists(_savePath)) return;
        using (Stream stream = File.Open(_savePath, FileMode.Open))
        {
            var bf = new BinaryFormatter();

            items = (List<string>)bf.Deserialize(stream);
        }

        if (items.Count < 1) return;

        _imagePaths = items;

        foreach (var image in _imagePaths)
        {
            var sprite = LoadNewSprite(image);
            if (sprite != null) _userWallpapers.Add(sprite);
        }
    }
}
