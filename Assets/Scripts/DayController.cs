using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

public class DayController : MonoBehaviour
{
    [SerializeField] private Days myDay;
    [SerializeField] private GameObject[] remainderPlaces;
    [SerializeField] private Canvas remainderCanvas;
    [SerializeField] private TMP_Dropdown day;
    [SerializeField] private TMP_Dropdown month;
    [SerializeField] private TMP_Dropdown year;
    [SerializeField] private TMP_Dropdown hour;
    [SerializeField] private TMP_Dropdown minute;
    [SerializeField] private TMP_InputField remainderText;
    [SerializeField] private Button saveButton;
    [SerializeField] private Canvas rightClickDayCanvas;
    [SerializeField] private Button addButton;
    [SerializeField] private Button showButton;
    [SerializeField] private Light2D ringLight;

    private Camera camera;

    private SortedList<int, BellController> _sortedRemainderPlaces;
    private SortedList<int, RemainderContainer> _sortedRemainders;
    private List<GameObject> _activeRemainders;
    private List<GameObject> _emptyRemainder;

    private Transform rightClickTransform;
    void Awake()
    {
        _sortedRemainders = new SortedList<int, RemainderContainer>();
        _sortedRemainderPlaces = new SortedList<int, BellController>();
        _activeRemainders = new List<GameObject>();
        _emptyRemainder = new List<GameObject>();

        addButton.onClick.AddListener(AddOnClick);
        showButton.onClick.AddListener(ShowOnClick);

        camera = Camera.main;
        rightClickTransform = rightClickDayCanvas.transform.GetChild(0);
        CheckDay();
        DigitalClock.OnDayChanged += CheckDay;
    }
    void Start()
    {
        CreateSortedRemainderPlaces();
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

    private void CreateSortedRemainderPlaces()
    {
        List<BellController> bellList = new List<BellController>();

        foreach (var bell in remainderPlaces)
        {
            bellList.Add(bell.GetComponent<BellController>());
        }

        for (int i = 0; i < bellList.Count; i++)
        {
            _sortedRemainderPlaces.Add(i, bellList[i]);
        }
    }

    public void AddRemainder(RemainderContainer remainder)
    {
        List<RemainderContainer> temp = new List<RemainderContainer>();
        SortedList<int, RemainderContainer> tempSorted = new SortedList<int, RemainderContainer>();
        if (_sortedRemainders.Count > 0)
        {
            foreach (var item in _sortedRemainders.Values)
            {
                temp.Add(item);
            }
        }

        if (temp.Count > 0)
        {
            foreach (var item in temp)
            {
                var tempSpan = item.DateTime - DateTime.Now;
                int i = 0;
                while (true)
                {
                    try
                    {
                        tempSorted.Add(Convert.ToInt32(tempSpan.TotalSeconds + i), item);
                        break;
                    }
                    catch (Exception e)
                    {
                        i += 1;
                    }
                }

            }
        }

        TimeSpan timeSpan = remainder.DateTime - DateTime.Now;

        int counter = 0;
        while (true)
        {
            try
            {
                tempSorted.Add(Convert.ToInt32(timeSpan.TotalSeconds + counter), remainder);
                break;
            }
            catch (Exception e)
            {
                counter += 1;
            }
        }

        _sortedRemainders.Clear();
        _sortedRemainders = tempSorted;
        RefreshRemainders();
    }

    public void RemoveRemainder(RemainderContainer remainder)
    {
        var index = _sortedRemainders.IndexOfValue(remainder);
        _sortedRemainders.Remove(index);
        RefreshRemainders();
    }

    private void SetActiveAllActiveRemainders()
    {
        foreach (var remainder in _activeRemainders)
        {
            remainder.SetActive(true);
        }
    }

    private void FillEmptyRemainders()
    {
        _emptyRemainder.Clear();
        _activeRemainders.Clear();
        foreach (var remainder in remainderPlaces)
        {
            _emptyRemainder.Add(remainder);
            remainder.SetActive(false);
        }
    }

    private void AddOnClick()
    {
        CanvasController.Instance.CloseActiveCanvas();
        remainderCanvas?.gameObject?.SetActive(true);
        CanvasController.IsMainCanvasOpen = true;
        saveButton.onClick.AddListener(SaveOnClick);
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

        AddRemainder(remainder);
        remainderCanvas.gameObject.SetActive(false);
        remainderText.text = "";
        CanvasController.IsMainCanvasOpen = false;
        saveButton.onClick.RemoveAllListeners();
    }

    private void RefreshRemainders()
    {
        FillEmptyRemainders();
        for (int i = 0; i < _sortedRemainderPlaces.Count; i++)
        {
            if (i >= _sortedRemainders.Count) break;
            var tempRemainder = _sortedRemainderPlaces[i];
            tempRemainder.AddRemainder(_sortedRemainders.Values[i]);
            _activeRemainders.Add(tempRemainder.gameObject);
            _emptyRemainder.Remove(tempRemainder.gameObject);
        }
        SetActiveAllActiveRemainders();
    }
}
