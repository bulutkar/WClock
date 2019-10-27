using System;

namespace Controllers.Weather
{
    [Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
    }
}
