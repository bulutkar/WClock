using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Controllers.Weather;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;
        [SerializeField] public List<Sprite> wallpapers;
        [SerializeField] private Image background;
        private Sprite _defaultBg;
        private GameObject _activeCanvas = null;
        private SaveDataContainer _saveDataContainer;
        public static bool IsMainCanvasOpen;
        public bool isWallpaperDynamic;
        public int defaultBgIndex;

        private string _savePath;
        private void Awake()
        {
            Instance = this;
            isWallpaperDynamic = true;
            _savePath = Application.persistentDataPath + "/wallpaper.dat";
            _saveDataContainer = new SaveDataContainer();
            LoadData();
            if (isWallpaperDynamic) WeatherControl.OnWeatherChange += ChangeWallpaperDynamically;
        }
        private void OnDisable()
        {
            if (WeatherControl.OnWeatherChange != null) WeatherControl.OnWeatherChange -= ChangeWallpaperDynamically;
        }
        public void SetWallpaper()
        {
            if (defaultBgIndex >= wallpapers.Count) SetDefaultBg(0);
            else _defaultBg = wallpapers[defaultBgIndex];

            SetWallpaperInitially();
        }
        private void SetWallpaper(int index)
        {
            background.sprite = wallpapers[index];
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
            var weather = WeatherControl.Instance.GetWeatherInfo();
            if (weather.weather.Count < 1)
            {
                SetWallpaperAsDefault();
                return;
            }
            switch (weather.weather[0].main)
            {
                case "Clear":
                    SetWallpaper(0);
                    break;
                case "Rain":
                    SetWallpaper(1);
                    break;
                case "Snow":
                    SetWallpaper(2);
                    break;
                case "Clouds":
                    SetWallpaper(3);
                    break;
            }
        }
        public void SetDefaultBg(int index)
        {
            defaultBgIndex = index;
            _saveDataContainer.defaultBgIndex = defaultBgIndex;
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
            _saveDataContainer.isWallpaperDynamic = isWallpaperDynamic;
            if (dynamic) WeatherControl.OnWeatherChange += ChangeWallpaperDynamically;
            else
            {
                if (WeatherControl.OnWeatherChange != null) WeatherControl.OnWeatherChange -= ChangeWallpaperDynamically;
            }
            SaveData();
        }
        private void SaveData()
        {
            FileStream fs = new FileStream(_savePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _saveDataContainer);
            fs.Close();
        }
        private void LoadData()
        {
            if (!File.Exists(_savePath))
            {
                _saveDataContainer.defaultBgIndex = defaultBgIndex;
                _saveDataContainer.isWallpaperDynamic = isWallpaperDynamic;
                return;
            }
            using (Stream stream = File.Open(_savePath, FileMode.Open))
            {
                var bf = new BinaryFormatter();

                _saveDataContainer = (SaveDataContainer)bf.Deserialize(stream);
                isWallpaperDynamic = _saveDataContainer.isWallpaperDynamic;
                defaultBgIndex = _saveDataContainer.defaultBgIndex;
            }
        }
    }
}

