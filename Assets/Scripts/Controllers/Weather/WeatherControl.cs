using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace Controllers.Weather
{
    public class WeatherControl : MonoBehaviour
    {
        public static WeatherControl Instance;
        [SerializeField] private TextMeshProUGUI celsius;
        [SerializeField] private TextMeshProUGUI description;
        private const string ApiKey = "22c9d314e68de55921621a23be19292b";
        public string cityId = "745044";
        [SerializeField] private WeatherInfo weatherInfo;
        private string _previousWeather = "Clear";

        public static Action OnWeatherChange;

        private void Awake()
        {
            cityId = "745044";
            Instance = this;
        }

        private void Start()
        {
            InvokeRepeating(nameof(CheckWeather), 0, 600);
        }

        public IEnumerator GetWeather()
        {
            using (UnityWebRequest req = UnityWebRequest.Get(
                $"http://api.openweathermap.org/data/2.5/weather?id={cityId}&APPID={ApiKey}"))
            {
                yield return req.SendWebRequest();
                while (!req.isDone)
                    yield return null;
                var result = req.downloadHandler.data;
                var weatherJson = System.Text.Encoding.Default.GetString(result);
                var json = JsonUtility.ToJson(weatherJson);
                weatherInfo = JsonUtility.FromJson<WeatherInfo>(weatherJson);

                if (weatherInfo.weather.Count > 0)
                {
                    var temp = KelvinToCelsius(weatherInfo.main.temp);
                    weatherInfo.main.temp = temp;
                    weatherInfo.main.temp_max = KelvinToCelsius(weatherInfo.main.temp_max);
                    weatherInfo.main.temp_min = KelvinToCelsius(weatherInfo.main.temp_min);
                    celsius.text = temp + " C";
                    if (_previousWeather != weatherInfo.weather[0].main)
                    {
                        description.text = weatherInfo.weather[0].description;
                        OnWeatherChange?.Invoke();
                    }
                }

            }
        }

        public void CheckWeather()
        {
            StartCoroutine(GetWeather());
        }
        public WeatherInfo GetWeatherInfo()
        {
            return weatherInfo;
        }

        private int KelvinToCelsius(float kelvin)
        {
            return (int)(kelvin - 273.15f);

        }
    }
}
