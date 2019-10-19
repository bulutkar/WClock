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
    [SerializeField] private Light2D ringLight;

    private SortedList<int, BellController> _sortedRemainderPlaces;
    private SortedList<int, RemainderContainer> _sortedRemainders;
    private List<GameObject> _activeRemainders;
    private List<GameObject> _emptyRemainder;

    void Awake()
    {
        _sortedRemainders = new SortedList<int, RemainderContainer>();
        _sortedRemainderPlaces = new SortedList<int, BellController>();
        _activeRemainders = new List<GameObject>();
        _emptyRemainder = new List<GameObject>();
        CheckDay();
        DigitalClock.OnDayChanged += CheckDay;
    }
    void Start()
    {
        CreateSortedRemainderPlaces();
    }
    void OnDestroy()
    {
        if (DigitalClock.OnDayChanged != null) DigitalClock.OnDayChanged -= CheckDay;
    }

    public string GetMyDay()
    {
        return myDay.ToString().ToLower();
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

    public SortedList<int, RemainderContainer> GetSortedRemainderList()
    {
        return _sortedRemainders;
    }
    private void CheckDay()
    {
        DateTime dateTime = DateTime.Now;
        var day = dateTime.DayOfWeek.ToString().ToLower();
        var result = day.Equals(myDay.ToString().ToLower());
        ringLight.enabled = result;

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
