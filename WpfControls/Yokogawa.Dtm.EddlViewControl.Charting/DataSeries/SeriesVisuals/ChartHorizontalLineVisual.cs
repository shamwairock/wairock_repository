using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartHorizontalLineVisual : DataSeriesVisual
    {
        protected override void Render(DrawingContext dc, DataSeries series, Plot plot, Axis xAxis, Axis yAxis, IList<DataSeries> sources)
        {
            if (series == null || plot == null || plot.Points == null || xAxis == null || yAxis == null)
            {
                return;
            }

            var xlong = xAxis.Extent;
            var ylong = yAxis.Extent;

            var lineStart = xAxis.StartPixelsPos;
            var lineEnd = xAxis.StopPixelsPos;
            var points = plot.Points;
            foreach (var pt in points)
            {
                double y; // coordinates in pixels
                bool isPtInsideArea;
                try
                {
                    y = yAxis.ToPixels(pt.Y);
                    isPtInsideArea = (y >= 0.0 || y <= ylong);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (isPtInsideArea)
                {
                    // Line Geometry
                    LineGeometry geometry = new LineGeometry();
                    geometry.StartPoint = new Point(lineStart, y);
                    geometry.EndPoint = new Point(lineEnd, y);
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
