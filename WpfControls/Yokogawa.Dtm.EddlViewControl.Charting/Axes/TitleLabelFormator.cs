using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class TitleLabelFormator
    {
        public TitleLabelFormator(Typeface typeface, FlowDirection flowDirection, double fontSize, Brush labelBrush)
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

        public string Title { get; set; }

        public Func<string, string> CustomFormatter { get; set; }

        public FormattedText GetFormattedText()
        {
            return GetFormattedText(Title, Typeface, FlowDirection, FontSize, LabelBrush);
        }

        private FormattedText GetFormattedText(string label, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            return new FormattedText(label, CultureInfo.CurrentUICulture, flowDirection, typeface, fontSize, brush);
        }

        private string GetString(string value)
        {
            string text = null;
            if (CustomFormatter != null)
            {
                text = CustomFormatter(value);
            }
            if (text == null)
            {
                text = value;
            }
            return text;
        }
    }
}
