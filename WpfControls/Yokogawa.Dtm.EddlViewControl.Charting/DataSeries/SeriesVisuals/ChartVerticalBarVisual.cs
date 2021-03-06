﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartVerticalBarVisual : DataSeriesVisual
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
                double y; // coordinates in pixels
                double x; // coordinates in pixels
                bool isPtInsideArea;
                try
                {
                    x = xAxis.ToPixels(pt.X);
                    y = yAxis.ToPixels(pt.Y);
                    isPtInsideArea = isInsideArea(x, y, xlong, ylong);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                if (isPtInsideArea)
                {
                    var barWidth = series.BarWidth;

                    // Line Geometry
                    StreamGeometry geometry = new StreamGeometry();
                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        double xStart = x - barWidth / 2;
                        double xStop = xStart + barWidth;

                        double yStart = lineStart;
                        double yStop = y;

                        ctx.BeginFigure(new Point(xStart, yStart), true, true);
                        ctx.PolyLineTo(new Point[]
                        {
                            new Point(xStart, yStop),
                            new Point(xStop, yStop),
                            new Point(xStop, yStart)
                        }, false, false);
                    }
                    geometry.Freeze();

                    // Clipping region
                    RectangleGeometry clip;
                    clip = new RectangleGeometry(new Rect(0, 0, xlong, ylong));
                    dc.PushClip(clip);
                    dc.DrawGeometry(series.Pen != null ? series.Pen.Brush : Brushes.Black, null, geometry);
                }
            }
        }

        protected bool isInsideArea(double x, double y, double areaWidth, double areaHeight, double barWidth)
        {
            System.Diagnostics.Debug.Assert(areaWidth >= 0, "area.Width >= 0");
            System.Diagnostics.Debug.Assert(areaHeight >= 0, "area.Height >= 0");

            if ( (x + barWidth / 2) < 0 || (x - barWidth / 2) > areaWidth || y < 0 || y > areaHeight)
            {
                return false;
            }
            return true;
        }
    }
}
