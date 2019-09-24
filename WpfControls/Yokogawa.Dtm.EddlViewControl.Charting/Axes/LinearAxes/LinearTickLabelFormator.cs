using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearTickLabelFormator
    {
        public LinearTickLabelFormator(Typeface typeface, FlowDirection flowDirection, double fontSize, Brush labelBrush)
        {
            Typeface = typeface;
            FlowDirection = flowDirection;
            FontSize = fontSize;
            LabelBrush = labelBrush;
        }

        private bool shouldRound = true;

        private int rounding;

        public Typeface Typeface { get; set; }

        public FlowDirection FlowDirection { get; set; }

        public double FontSize { get; set; }

        public Brush LabelBrush { get; set; }

        public Func<object, string> CustomFormatter { get; set; }

        public string LabelStringFormat { get; set; }

        public void InitTicks(double[] ticks)
        {
            if (ticks == null || ticks.Length == 0)
            {
                return;
            }
            shouldRound = true;
            try
            {
                double start = ticks[0];
                double finish = ticks[ticks.Length - 1];
                if (start == finish)
                {
                    shouldRound = false;
                    return;
                }
                double delta = finish - start;
                rounding = (int)Math.Round(Math.Log10(delta));
                double newStart = RoundHelper.Round(start, rounding);
                double newFinish = RoundHelper.Round(finish, rounding);
                if (newStart == newFinish)
                {
                    rounding--;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public FormattedText GetFormattedText(object value)
        {
            return GetFormattedText(GetString(value), Typeface, FlowDirection, FontSize, LabelBrush);
        }

        private FormattedText GetFormattedText(string label, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            return new FormattedText(label, CultureInfo.CurrentUICulture, flowDirection, typeface, fontSize, brush);
        }

        private string GetString(object value)
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

        private string GetStringCore(object value)
        {
            string res;
            if(value is double && shouldRound)
            {
                int round = Math.Min(15, Math.Max(-15, rounding - 2));
                res = RoundHelper.Round((double)value, round).ToString();
            }
            else
            {
                res = value.ToString();
            }            
            return res;
        }
    }
}
