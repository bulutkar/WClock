using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherInfo
{
    public int id;
    public string name;
    public List<Weather> weather;
    public Main main;
}
