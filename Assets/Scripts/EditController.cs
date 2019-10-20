using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditController : MonoBehaviour
{
    public static EditController Instance;
    [SerializeField] private DayController[] dayControllers;
    [SerializeField] private Canvas remainderCanvas;
    [SerializeField] private TMP_InputField remainderText;
    [SerializeField] private TMP_Dropdown day;
    [SerializeField] private TMP_Dropdown month;
    [SerializeField] private TMP_Dropdown year;
    [SerializeField] private TMP_Dropdown hour;
    [SerializeField] private TMP_Dropdown minute;
    [SerializeField] public Button saveButton;
    [SerializeField] private Toggle alarmToggle;

    [HideInInspector] public BellController BellController;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadCanvas(RemainderContainer remainder)
    {
        remainderText.text = remainder.Text;
        CanvasController.Instance.CloseActiveCanvas();
        remainderCanvas?.gameObject?.SetActive(true);
        CanvasController.IsMainCanvasOpen = true;
    }
    public void SaveOnClick()
    {
        RemainderContainer remainder = new RemainderContainer();
        remainder.Text = remainderText.text;
        remainder.Year = Convert.ToInt32(year.options[year.value].text);
        remainder.Month = Convert.ToInt32(month.options[month.value].text);
        remainder.Day = Convert.ToInt32(day.options[day.value].text);
        remainder.Hour = Convert.ToInt32(hour.options[hour.value].text);
        remainder.Minute = Convert.ToInt32(minute.options[minute.value].text);
        remainder.DateTime = new DateTime(remainder.Year, remainder.Month, remainder.Day, remainder.Hour, remainder.Minute, 0);
        remainder.Alarm = alarmToggle.isOn;
        var index = FindTheDayController(remainder.DateTime.DayOfWeek.ToString());
        dayControllers[index].AddRemainder(remainder);
        remainderCanvas.gameObject.SetActive(false);
        remainderText.text = "";
        CanvasController.IsMainCanvasOpen = false;
    }
    public void CloseCanvas()
    {
        saveButton.onClick.RemoveAllListeners();
        remainderCanvas.gameObject.SetActive(false);
        CanvasController.IsMainCanvasOpen = false;
    }
    private int FindTheDayController(string dayOfWeek)
    {
        int index = 0;
        for (index = 0; index < dayControllers.Length; index++)
        {
            if (dayControllers[index].GetMyDay() == dayOfWeek.ToLower()) break;
        }

        return index;
    }
}
