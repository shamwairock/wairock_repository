using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DoubleAxisData : IAxisDataType
    {
        public virtual double ToAxisDouble(object value)
        {
            return Convert.ToDouble(value);
        }

        public virtual object ToDataValue(double value)
        {
            return value;
        }

        public virtual double DataLengthToDoubleAxisLength(object value)
        {
            return Convert.ToDouble(value);
        }

        public int Compare(object valueOne, object valueTwo)
        {
            var done = Convert.ToDouble(valueOne);
            var dtwo = Convert.ToDouble(valueTwo);
            return done.CompareTo(dtwo);
        }

        public bool ValidData(object value)
        {
            try
            {
                var v = Convert.ToDouble(value);
                return !double.IsNaN(v);
            }
            catch { }
            return false;
        }
    }

}
