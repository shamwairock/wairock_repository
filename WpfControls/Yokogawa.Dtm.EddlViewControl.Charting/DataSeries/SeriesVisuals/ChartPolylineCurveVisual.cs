using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartPolylineCurveVisual : DataSeriesVisual
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

            var xlong = xAxis.Extent;
            var ylong = yAxis.Extent;

            Point? startPoint = null;
            IDataPoint startDataPoint = null;
            List<Point> linePoints = new List<Point>();

            var points = plot.Points;
            foreach (var pt in points)
            {
                double x, y; // coordinates in pixels
                bool isPtInsideArea = false;
                try
                {
                    x = xAxis.ToPixels(pt.X, plot.XScale);
                    y = yAxis.ToPixels(pt.Y, plot.XScale);
                    isPtInsideArea = isInsideArea(x, y, xlong, ylong);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (!startPoint.HasValue)
                {
                    startPoint = new Point(x, y);
                    startDataPoint = pt;
                }
                else
                {
                    linePoints.Add(new Point(x, y));
                }

                if (isPtInsideArea)
                {
                    Drawing pointMarker = iPointMarker.PointMarkerVisible ? iPointMarker.PointMarker : null;
                    pointMarker = pt.Emphasis ? iPointMarker.EmphasisPointMarker : pointMarker;
                    DrawPointsMarker(pointMarker, x, y);
                }
            }

            if (!startPoint.HasValue)
            {
                return;
            }

            if (linePoints.Count == 0 && isInsideArea(startPoint.Value.X, startPoint.Value.Y, xlong, ylong))
            {
                Drawing pointMarker = iPointMarker.PointMarker;
                pointMarker = startDataPoint.Emphasis ? iPointMarker.EmphasisPointMarker : pointMarker;
                DrawPointsMarker(pointMarker, startPoint.Value.X, startPoint.Value.Y);
                return;
            }

            var clipRect = PlotHelper.GetClipRect(plot.ClipSettings, xAxis, yAxis, xlong, ylong);
            var gCurve = CreateCurve(startPoint.Value, linePoints);
            DrawCurve(dc, gCurve, series.Pen, clipRect);
        }

        private void DrawCurve(DrawingContext dc, Geometry geometry, Pen pen, Rect clipRect)
        {
            // Clipping region
            RectangleGeometry clip;
            clip = new RectangleGeometry(clipRect);
            dc.PushClip(clip);
            dc.DrawGeometry(Brushes.Transparent, pen, geometry);
            dc.Pop();
        }

        private Geometry CreateCurve(Point startPoint, List<Point> linePoints)
        {
            // Curve figure geometry
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(startPoint, false /* is filled */, false /* is closed */);
                ctx.PolyLineTo(linePoints, true /* is stroked */, true /* is smooth join */);
            }
            geometry.Freeze();
            return geometry;
        }
    }
}
