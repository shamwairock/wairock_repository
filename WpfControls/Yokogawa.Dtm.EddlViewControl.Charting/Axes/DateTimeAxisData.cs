using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DateTimeAxisData : IAxisDataType
    {
        public virtual double ToAxisDouble(object value)
        {
            DateTime time = Convert.ToDateTime(value);
            return time.Ticks;
        }

        public virtual object ToDataValue(double value)
        {
            return new DateTime((long)value);
        }

        public virtual double DataLengthToDoubleAxisLength(object value)
        {
            if (value is TimeSpan)
            {
                return ((TimeSpan)value).Ticks;
            }
            return 0.0;
        }

        public int Compare(object valueOne, object valueTwo)
        {
            var done = Convert.ToDateTime(valueOne);
            var dtwo = Convert.ToDateTime(valueTwo);
            return done.CompareTo(dtwo);
        }

        public bool ValidData(object value)
        {
            try
            {
                Convert.ToDateTime(value);
                return true;
            }
            catch { }
            return false;
        }
    }

}
