using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartVerticalLineVisual : DataSeriesVisual
    {
        protected override void Render(DrawingContext dc, DataSeries series, Plot plot, Axis xAxis, Axis yAxis, IList<DataSeries> sources)
        {
            if (series == null || plot == null || plot.Points == null || xAxis == null || yAxis == null)
            {
                return;
            }

            var xlong = xAxis.Extent;
            var ylong = yAxis.Extent;

            var lineStart = yAxis.StartPixelsPos;
            var lineEnd = yAxis.StopPixelsPos;

            var points = plot.Points;
            foreach (var pt in points)
            {
                double x; // coordinates in pixels
                bool isPtInsideArea;
                try
                {
                    x = xAxis.ToPixels(pt.X);
                    isPtInsideArea = (x >= 0.0 || x <= xlong);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (isPtInsideArea)
                {
                    // Line Geometry
                    LineGeometry geometry = new LineGeometry();
                    geometry.StartPoint = new Point(x, lineStart);
                    geometry.EndPoint = new Point(x, lineEnd);                   
                    geometry.Freeze();

                    // Clipping region
                    RectangleGeometry clip;
                    clip = new RectangleGeometry(new Rect(0, 0, xlong, ylong));                    
                    dc.PushClip(clip);
                    dc.DrawGeometry(Brushes.Transparent, series.Pen, geometry);
                }
            }
        }
    }
}
