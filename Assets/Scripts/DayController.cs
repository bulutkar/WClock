using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

public class DayController : MonoBehaviour
{
    [SerializeField] private Days myDay;
    [SerializeField] private GameObject[] remainderPlaces;
    [SerializeField] private Canvas remainderCanvas;
    [SerializeField] private Canvas rightClickDayCanvas;
    [SerializeField] private Light2D ringLight;
    void Awake()
    {
        CheckDay();
        DigitalClock.OnDayChanged += CheckDay;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckDay()
    {
        DateTime dateTime = DateTime.Now;
        var day = dateTime.DayOfWeek.ToString().ToLower();
        var result = day.Equals(myDay.ToString().ToLower());
        ringLight.enabled = result;

    }

    private void OnDestroy()
    {
        if (DigitalClock.OnDayChanged != null) DigitalClock.OnDayChanged -= CheckDay;
    }
}
