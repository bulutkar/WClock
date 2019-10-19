using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeekController : MonoBehaviour
{
    [SerializeField] private DayController[] dayControllers;
    [SerializeField] private Canvas remainderCanvas;
    [SerializeField] private TMP_InputField remainderText;
    [SerializeField] private TMP_Dropdown day;
    [SerializeField] private TMP_Dropdown month;
    [SerializeField] private TMP_Dropdown year;
    [SerializeField] private TMP_Dropdown hour;
    [SerializeField] private TMP_Dropdown minute;
    [SerializeField] private Button saveButton;
    [SerializeField] private Canvas rightClickDayCanvas;
    [SerializeField] private Button addButton;
    [SerializeField] private Button showButton;

    private Camera camera;
    private Transform rightClickTransform;
    private void Awake()
    {
        addButton.onClick.AddListener(AddOnClick);
        showButton.onClick.AddListener(ShowOnClick);
        saveButton.onClick.AddListener(SaveOnClick);
        rightClickTransform = rightClickDayCanvas.transform.GetChild(0);
        camera = Camera.main;
    }

    private void OnMouseOver()
    {
        if (CanvasController.IsMainCanvasOpen) return;
        if (Input.GetMouseButtonDown(1))
        {
            CanvasController.Instance.CloseActiveCanvas();
            var mousePos = Input.mousePosition;
            mousePos.z = rightClickDayCanvas.transform.position.z;
            rightClickTransform.position = camera.ScreenToWorldPoint(mousePos) + Vector3.right * 0.5f;
            rightClickDayCanvas.gameObject?.SetActive(true);
            CanvasController.Instance.AddActiveCanvas(rightClickDayCanvas.gameObject);
        }
    }
    private void OnDestroy()
    {
        addButton.onClick.RemoveAllListeners();
        showButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
    }

    private void AddOnClick()
    {
        CanvasController.Instance.CloseActiveCanvas();
        remainderCanvas?.gameObject?.SetActive(true);
        CanvasController.IsMainCanvasOpen = true;
    }

    private void ShowOnClick() { }

    private void SaveOnClick()
    {
        RemainderContainer remainder = new RemainderContainer();
        remainder.Text = remainderText.text;
        remainder.Year = Convert.ToInt32(year.options[year.value].text);
        remainder.Month = Convert.ToInt32(month.options[month.value].text);
        remainder.Day = Convert.ToInt32(day.options[day.value].text);
        remainder.Hour = Convert.ToInt32(hour.options[hour.value].text);
        remainder.Minute = Convert.ToInt32(minute.options[minute.value].text);
        remainder.DateTime = new DateTime(remainder.Year, remainder.Month, remainder.Day, remainder.Hour, remainder.Minute, 0);
        var index = FindTheDayController(remainder.DateTime.DayOfWeek.ToString());
        dayControllers[index].AddRemainder(remainder);
        remainderCanvas.gameObject.SetActive(false);
        remainderText.text = "";
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
