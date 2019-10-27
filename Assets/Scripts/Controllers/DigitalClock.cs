using System;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class DigitalClock : MonoBehaviour
    {
        public static Action OnDayChanged;
        private TextMeshProUGUI _tmpText;

        private DayOfWeek _day;

        private void Awake()
        {
            _tmpText = GetComponent<TextMeshProUGUI>();
            DateTime time = DateTime.Now;
            _day = time.DayOfWeek;

        }

        void Update()
        {
            DateTime time = DateTime.Now;
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            _tmpText.text = hour + ":" + minute + ":" + second;
            if (_day != time.DayOfWeek) OnDayChanged?.Invoke();
            _day = time.DayOfWeek;
        }
    }
}
