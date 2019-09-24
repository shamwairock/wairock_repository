using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearTickLabelDrawing : TickLabelDrawing
    {
        public override Size LabelSize(object value, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            if (value == null)
            {
                return new Size(0, 0);
            }
            var ftLabel = GetFormattedText(GetString(value), typeface, flowDirection, fontSize, brush);
            return new Size(ftLabel.Width, ftLabel.Height);
        }

        public override Size DrawLabel(DrawingContext dc, object value, double labelPos, double labelTop, Typeface typeface, FlowDirection flowDirection, double fontSize, Brush brush)
        {
            if (value == null)
            {
                return new Size(0, 0);
            }
            var ftLabel = GetFormattedText(GetString(value), typeface, flowDirection, fontSize, brush);
            dc.DrawText(ftLabel, new Point(labelPos, labelTop));
            return new Size(ftLabel.Width, ftLabel.Height);
        }
    }
}
