using System;
using System.Collections.Generic;

namespace Controllers.Weather
{
    [Serializable]
    public class WeatherInfo
    {
        public int id;
        public string name;
        public List<Weather> weather;
        public Main main;
    }
}
