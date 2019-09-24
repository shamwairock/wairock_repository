using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartScatteredPointsVisual : DataSeriesVisual
    {
        protected override void Render(DrawingContext dc, DataSeries series, Plot plot, Axis xAxis, Axis yAxis, IList<DataSeries> sources)
        {
            if (series == null || plot == null || plot.Points == null || xAxis == null || yAxis == null)
            {
                return;
            }

            // Curve point marker drawing.
            IPointMarker iPointMarker = series as IPointMarker;
            Debug.Assert(iPointMarker != null, "iPointMarker != null");

            var hlong = xAxis.Extent;
            var vlong = yAxis.Extent;

            var points = plot.Points;
            foreach (var pt in points)
            {
                double x, y; // coordinates in pixels
                bool isPtInsideArea;
                try
                {
                    x = xAxis.ToPixels(pt.X);
                    y = yAxis.ToPixels(pt.Y);
                    isPtInsideArea = isInsideArea(x, y, hlong, vlong);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (isPtInsideArea)
                {
                    Drawing pointMarker = iPointMarker.PointMarkerVisible ? iPointMarker.PointMarker : null;
                    pointMarker = pt.Emphasis ? iPointMarker.EmphasisPointMarker : pointMarker;
                    DrawPointsMarker(pointMarker, x, y);
                }
            }
        }
    }
}
