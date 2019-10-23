using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeekController : MonoBehaviour
{
    public static WeekController Instance;
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
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Toggle alarmToggle;

    [SerializeField] private GameObject allRemainderListGameObject;
    [SerializeField] private Transform viewParent;
    [SerializeField] private GameObject remainderView;

    private Camera camera;
    private Transform rightClickTransform;
    private void Awake()
    {
        Instance = this;
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

    public void ShowTheDay(int index)
    {
        var controller = dayControllers[index];
        var sortedList = controller.GetSortedRemainderList();
        var permanentIndex = index;
        for (int i = 0; i < viewParent.childCount; i++)
        {
            var obj = viewParent.GetChild(i).gameObject;
            obj.SetActive(false);
            Destroy(obj);
        }
        for (int i = 0; i < sortedList.Count; i++)
        {
            var everyIndex = i;
            GameObject view = Instantiate(remainderView, viewParent);

            Vector3 remainPos = view.transform.localPosition;
            remainPos.y -= i * 110;

            view.transform.localPosition = remainPos;
            TextMeshProUGUI dayText = view.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI timeText = view.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI contextText = view.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Button delButton = view.transform.GetChild(3).GetComponent<Button>();
            Button editButton = view.transform.GetChild(4).GetComponent<Button>();

            dayText.text = sortedList.Values[i].DateTime.ToShortDateString();
            timeText.text = sortedList.Values[i].DateTime.ToShortTimeString();
            contextText.text = sortedList.Values[i].Text;
            delButton.onClick.AddListener((() =>
            {
                controller.RemoveRemainder(sortedList.Values[everyIndex]);
                ShowTheDay(permanentIndex);
            }));
            editButton.onClick.AddListener((() =>
            {
                CanvasController.Instance.CloseActiveCanvas();
                CanvasController.IsMainCanvasOpen = false;
                allRemainderListGameObject.SetActive(false);
                EditController.Instance.saveButton.onClick.AddListener((() =>
                {
                    controller.RemoveRemainder(sortedList.Values[everyIndex]);
                    EditController.Instance.SaveOnClick();
                    EditController.Instance.saveButton.onClick.RemoveAllListeners();
                }));
                EditController.Instance.LoadCanvas(sortedList.Values[everyIndex]);
                ShowTheDay(permanentIndex);
            }));
        }

    }

    private void AddOnClick()
    {
        CanvasController.Instance.CloseActiveCanvas();
        remainderCanvas.gameObject.SetActive(true);
        CanvasController.IsMainCanvasOpen = true;
    }

    private void ShowOnClick()
    {
        CanvasController.Instance.CloseActiveCanvas();
        CanvasController.IsMainCanvasOpen = true;
        allRemainderListGameObject.SetActive(true);
        ShowTheDay(0);
    }

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
        remainder.Alarm = alarmToggle.isOn;
        var index = FindTheDayController(remainder.DateTime.DayOfWeek.ToString());
        dayControllers[index].AddRemainder(remainder);
        remainderCanvas.gameObject.SetActive(false);
        remainderText.text = "";
        CanvasController.IsMainCanvasOpen = false;
    }
    private RemainderContainer CreateRemainder(string text, int year, int month, int day, int hour, int minute, bool alarm)
    {
        RemainderContainer remainder = new RemainderContainer();
        remainder.Text = text;
        remainder.Year = year;
        remainder.Month = month;
        remainder.Day = day;
        remainder.Hour = hour;
        remainder.Minute = minute;
        remainder.DateTime = new DateTime(year, month, day, hour, minute, 0);
        remainder.Alarm = alarm;
        return remainder;
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
