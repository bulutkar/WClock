using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Containers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

namespace Controllers
{
    public class DayController : MonoBehaviour
    {
        [SerializeField] private Days myDay;
        [SerializeField] private GameObject[] remainderPlaces;
        [SerializeField] private Light2D ringLight;

        private SortedList<int, BellController> _sortedRemainderPlaces;
        private SortedList<int, RemainderContainer> _sortedRemainders;
        private List<RemainderContainer> _remainderList;
        private List<GameObject> _activeRemainders;
        private List<GameObject> _emptyRemainder;

        private string _savePath;

        void Awake()
        {
            _savePath = Application.persistentDataPath + "/" + myDay + "save.dat";
            _sortedRemainders = new SortedList<int, RemainderContainer>();

            _sortedRemainderPlaces = new SortedList<int, BellController>();
            CreateSortedRemainderPlaces();

            _activeRemainders = new List<GameObject>();
            _emptyRemainder = new List<GameObject>();
            _remainderList = new List<RemainderContainer>();
            CheckDay();
            DigitalClock.OnDayChanged += CheckDay;
        }

        private void Start()
        {
            LoadData();
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
            FillRemainderList();
            RefreshRemainders();
            SaveData();
        }

        public void AddRemainderInLoad(RemainderContainer remainder)
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
            FillRemainderList();
            RefreshRemainders();
        }
        public void RemoveRemainder(RemainderContainer remainder)
        {
            var index = _sortedRemainders.IndexOfValue(remainder);
            _sortedRemainders.RemoveAt(index);
            RefreshRemainders();
            FillRemainderList();
            SaveData();
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
                _activeRemainders.Add(tempRemainder.gameObject);
                tempRemainder.AddRemainder(_sortedRemainders.Values[i]);
                tempRemainder.gameObject.SetActive(true);
                _emptyRemainder.Remove(tempRemainder.gameObject);
            }
            SetActiveAllActiveRemainders();
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
        private void FillRemainderList()
        {
            _remainderList.Clear();
            foreach (var remainder in _sortedRemainders.Values)
            {
                _remainderList.Add(remainder);
            }
        }

        private void SaveData()
        {
            FileStream fs = new FileStream(_savePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _remainderList);
            fs.Close();
        }
        private void LoadData()
        {
            List<RemainderContainer> items = new List<RemainderContainer>();
            if (!File.Exists(_savePath)) return;
            using (Stream stream = File.Open(_savePath, FileMode.Open))
            {
                var bf = new BinaryFormatter();

                items = (List<RemainderContainer>)bf.Deserialize(stream);
            }

            if (items.Count < 1) return;
            foreach (var remainder in items)
            {
                AddRemainderInLoad(remainder);
            }
        }
    }
}
