using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DataPointVisual : DrawingVisual
    {
        public DataPointVisual(Drawing marker)
        {
            using (DrawingContext dc = RenderOpen())
            {
                dc.DrawDrawing(marker);
            }
        }
    }
}
