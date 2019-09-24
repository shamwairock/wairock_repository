using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartBezierCurveVisual : DataSeriesVisual
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

            Point? startPoint = null;
            IDataPoint startDataPoint = null;

            List<Point> linePoints = new List<Point>();
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

            if (linePoints.Count == 0 && isInsideArea(startPoint.Value.X, startPoint.Value.Y, hlong, vlong))
            {
                Drawing pointMarker = iPointMarker.PointMarker;
                pointMarker = startDataPoint.Emphasis ? iPointMarker.EmphasisPointMarker : pointMarker;
                DrawPointsMarker(pointMarker, startPoint.Value.X, startPoint.Value.Y);
            }

            // Bezier points
            Point[] bezierPoints = BezierPoints(linePoints.ToArray());

            if (bezierPoints.Length == 0)
            {
                return; // Nothing to draw
            }

            var clipRect = PlotHelper.GetClipRect(plot.ClipSettings, xAxis, yAxis, hlong, vlong);
            var gCurve = CreateCurve(startPoint.Value, linePoints);
            DrawCurve(dc, gCurve, series.Pen, clipRect);
        }

        private static Point[] BezierPoints(Point[] points)
        {
            int n = points.Length - 1;
            if (n < 1)
                return new Point[0];

            // Calculate first Bezier control points
            // Right hand side vector
            double[] r = new double[n];

            // Set right hand side X values
            for (int i = 1; i < n - 1; ++i)
                r[i] = 4 * points[i].X + 2 * points[i + 1].X;
            r[0] = points[0].X + 2 * points[1].X;
            r[n - 1] = 3 * points[n - 1].X;
            // Get first control points X-values
            double[] x = Solve(r);

            // Set right hand side Y values
            for (int i = 1; i < n - 1; ++i)
                r[i] = 4 * points[i].Y + 2 * points[i + 1].Y;
            r[0] = points[0].Y + 2 * points[1].Y;
            r[n - 1] = 3 * points[n - 1].Y;
            // Get first control points Y-values
            double[] y = Solve(r);

            Point[] bezierPoints = new Point[n * 3];
            for (int i = 0; i < n; ++i)
            {
                // First control point
                bezierPoints[3 * i] = new Point(x[i], y[i]);
                // Second control point
                if (i < n - 1)
                    bezierPoints[3 * i + 1] = new Point(2 * points[i + 1].X - x[i + 1], 2 * points[i + 1].Y - y[i + 1]);
                else
                    bezierPoints[3 * i + 1] = new Point((points[n].X + x[n - 1]) / 2, (points[n].Y + y[n - 1]) / 2);
                // Bezier knot point
                bezierPoints[3 * i + 2] = points[i + 1];
            }

            return bezierPoints;
        }

        static double[] Solve(double[] rhs)
        {
            int n = rhs.Length;
            double[] x = new double[n]; // Solution vector.
            double[] tmp = new double[n]; // Temp workspace.

            double b = 2.0;
            x[0] = rhs[0] / b;
            for (int i = 1; i < n; i++) // Decomposition and forward substitution.
            {
                tmp[i] = 1 / b;
                b = (i < n - 1 ? 4.0 : 2.0) - tmp[i];
                Debug.Assert(b != 0.0, "b != 0.0");
                x[i] = (rhs[i] - x[i - 1]) / b;
            }
            for (int i = 1; i < n; i++)
                x[n - i - 1] -= tmp[n - i] * x[n - i]; // Backsubstitution.

            return x;
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
