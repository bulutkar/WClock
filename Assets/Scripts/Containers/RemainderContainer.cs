using System;

namespace Containers
{
    [Serializable]
    public class RemainderContainer
    {
        public DateTime DateTime;
        public string Text;
        public int Day;
        public int Month;
        public int Year;
        public int Hour;
        public int Minute;
        public bool Alarm;
    }
}
