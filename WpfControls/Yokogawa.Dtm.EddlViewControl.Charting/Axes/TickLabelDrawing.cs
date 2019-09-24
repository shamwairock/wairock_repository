using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class TickLabelDrawing
    {
        public abstract Size LabelSize(object value, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush);
       
        public abstract Size DrawLabel(DrawingContext dc, object value, double labelPos, double labelTop, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush);

        public static FormattedText GetFormattedText(string label, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            return new FormattedText(label, CultureInfo.CurrentUICulture, flowDirection, typeface, fontSize, brush);
        }

        public Func<object, string> CustomFormatter { get; set; }

        public string LabelStringFormat { get; set; }

        public virtual string[] CreateLabels(TickInfo[] ticks)
        {
            return ticks.Select(item => GetString(item.Value)).ToArray();
        }

        protected virtual string GetString(object value)
        {
            string text = null;
            if (CustomFormatter != null)
            {
                text = CustomFormatter(value);
            }
            if (text == null)
            {
                text = GetStringCore(value);

                if (text == null)
                    throw new ArgumentNullException("");
            }
            if (LabelStringFormat != null)
            {
                text = String.Format(LabelStringFormat, text);
            }

            return text;
        }

        bool shouldRound = true;
        private int rounding;
        public void InitTicks(TickInfo[] ticks)
        {
            if (ticks == null || ticks.Length == 0 || (!(ticks.First().Value is double)))
            {
                return;
            }

            shouldRound = true;
            try
            {
                double start = (double)ticks[0].Value;
                double finish = (double)ticks[ticks.Length - 1].Value;

                if (start == finish)
                {
                    shouldRound = false;
                    DebugTraceLog.WriteLine("shouldRound = false;");
                    return;
                }

                double delta = finish - start;

                rounding = (int)Math.Round(Math.Log10(delta));

                double newStart = RoundHelper.Round(start, rounding);
                double newFinish = RoundHelper.Round(finish, rounding);
                if (newStart == newFinish)
                    rounding--;
            }
            catch
            {
            }
        }

        protected virtual string GetStringCore(object value)
        {
            if (value is double)
            {
                string res;
                if (!shouldRound)
                {
                    res = value.ToString();
                }
                else
                {
                    int round = Math.Min(15, Math.Max(-15, rounding - 2));   
                    res = RoundHelper.Round((double)value, round).ToString();
                }

                return res;
            }
            else
            {
                return value.ToString();
            }
        }
 
    }
}
