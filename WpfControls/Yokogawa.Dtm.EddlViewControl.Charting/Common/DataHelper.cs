using System;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public static class DataHelper
    {
        public static bool IsValidDouble(object value)
        {
            if (value == null)
            {
                return false;
            }

            try
            {
                double v = Convert.ToDouble(value);
                if (double.IsInfinity(v) || double.IsNaN(v))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsValidDateTime(object value)
        {
            if (value == null)
            {
                return false;
            }
            return (value.GetType() == typeof(DateTime));
        }

        public static bool IsValidTimeSpan(object value)
        {
            if (value == null)
            {
                return false;
            }
            return (value.GetType() == typeof(TimeSpan));
        }      
    }
}
