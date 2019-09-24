using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicTickLabelFormator
    {
        public LogarithmicTickLabelFormator(Typeface typeface, FlowDirection flowDirection, double fontSize, Brush labelBrush)
        {
            Typeface = typeface;
            FlowDirection = flowDirection;
            FontSize = fontSize;
            LabelBrush = labelBrush;
        }

        public Typeface Typeface { get; set; }

        public FlowDirection FlowDirection { get; set; }

        public double FontSize { get; set; }

        public Brush LabelBrush { get; set; }

        public Func<object, string> CustomFormatter { get; set; }

        public string LabelStringFormat { get; set; }

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
            return value.ToString();
        }
    }
}
