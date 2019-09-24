using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicTickLabelDrawing : TickLabelDrawing
    {
        public override Size LabelSize(object value, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            if (value == null)
            {
                return new Size(0, 0);
            }

            System.Diagnostics.Debug.Assert(value is double);

            try
            {
                string label =  Math.Log10((double)value).ToString();
                FormattedText ftLabel10 = GetFormattedText("10", typeface, flowDirection, fontSize, brush);
                FormattedText ftLabel = GetFormattedText(label, typeface, flowDirection, fontSize / 1.5, brush);

                return new Size(ftLabel10.Width + ftLabel.Width + 1.0, ftLabel10.Height);
            }
            catch
            {
                return new Size(0, 0);
            }
        }

        public override Size DrawLabel(DrawingContext dc, object value, double labelPos, double labelTop, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            if (value == null)
            {
                return new Size(0, 0);
            }

            System.Diagnostics.Debug.Assert(value is double);

            try
            {
                string label = Math.Log10((double)value).ToString();
                FormattedText ftLabel10 = GetFormattedText("10", typeface, flowDirection, fontSize, brush);
                FormattedText ftLabel = GetFormattedText(label, typeface, flowDirection, fontSize / 1.5, brush);

                dc.DrawText(ftLabel10, new Point(labelPos, labelTop));
                dc.DrawText(ftLabel, new Point(labelPos + ftLabel10.Width + 1.0, labelTop));

                return new Size(ftLabel10.Width + ftLabel.Width + 1.0, ftLabel10.Height);
            }
            catch
            {
                return new Size(0, 0);
            }
        }
    }
}
