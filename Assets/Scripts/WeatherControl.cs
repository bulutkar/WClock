using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class WeatherControl : MonoBehaviour
{
    public static WeatherControl Instance;
    [SerializeField] private TextMeshProUGUI celsius;
    [SerializeField] private TextMeshProUGUI description;
    private const string ApiKey = "22c9d314e68de55921621a23be19292b";
    public string cityId = "745044";
    [SerializeField] private WeatherInfo _weatherInfo;
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
            _weatherInfo = JsonUtility.FromJson<WeatherInfo>(weatherJson);

            if (_weatherInfo.weather.Count > 0)
            {
                var temp = KelvinToCelsius(_weatherInfo.main.temp);
                _weatherInfo.main.temp = temp;
                _weatherInfo.main.temp_max = KelvinToCelsius(_weatherInfo.main.temp_max);
                _weatherInfo.main.temp_min = KelvinToCelsius(_weatherInfo.main.temp_min);
                celsius.text = temp + " C";
                if (_previousWeather != _weatherInfo.weather[0].main)
                {
                    description.text = _weatherInfo.weather[0].description;
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
        return _weatherInfo;
    }

    private int KelvinToCelsius(float kelvin)
    {
        return (int)(kelvin - 273.15f);

    }
}
