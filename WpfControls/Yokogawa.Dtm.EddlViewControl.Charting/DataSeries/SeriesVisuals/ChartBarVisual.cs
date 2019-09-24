using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartBarVisual : DataSeriesVisual
    {
        //public override void Render(VisualContext context)
        //{
        //    Children.Clear();
        //    if (context == null)
        //    {
        //        return;
        //    }

        //    var xAxis = this.DataSeries.XAxis;
        //    var yAxis = this.DataSeries.YAxis;
        //    var sources = context.Sources;
        //    if (this.DataSeries != null && xAxis != null && yAxis != null && sources != null)
        //    {
        //        using (DrawingContext dc = RenderOpen())
        //        {
        //            if (this.DataSeries.IsVisible)
        //            {
        //                if (!sources.Contains(this.DataSeries))
        //                {
        //                    return;
        //                }
        //                int sourceIndex = sources.IndexOf(this.DataSeries);
        //                long tickCount = xAxis.TickCount;
        //                if (tickCount <= 1)
        //                {
        //                    return;
        //                }

        //                double tickInterval = xAxis.StopPos / (tickCount);
        //                //System.Diagnostics.Trace.WriteLine(string.Format("tickInterval = {0}", tickInterval));
        //                List<int> seriesCounts = (from src in sources
        //                                          where (src != null)
        //                                          select src).Count.ToList();

        //                int seriesCount = 0;
        //                int beforeSourceSeriesCount = 0;

        //                for (int i = 0; i < seriesCounts.Count; i++)
        //                {
        //                    seriesCount += seriesCounts[i];
        //                    if (i > 0 && i <= sourceIndex)
        //                    {
        //                        beforeSourceSeriesCount += seriesCounts[i - 1];
        //                    }
        //                }

        //                var barWidth = tickInterval / (seriesCount + 4); // reserve a portion at startValue and endValue
        //                //System.Diagnostics.Trace.WriteLine(string.Format("barWidth = {0}", barWidth));

        //                beforeSourceSeriesCount += 2; // added the portion portion

        //                int num = 0;
        //                foreach (var series in this.ChartSource.DataSeries)
        //                {
        //                    double offSet = barWidth * (beforeSourceSeriesCount + (num++)) - tickInterval / 2;

        //                    Render(dc, series as DataSeries, xAxis, yAxis, barWidth, offSet);
        //                }
        //            }
        //        }
        //    }
        //}

        //private double CalculateBarWidth(DataSeries series, XAxis xAxis)
        //{
        //    try
        //    {
        //        IEnumerable<Plot> plots = xAxis.ToPlot(series.Points);
        //        double retBarWidth = double.NaN;
        //        foreach (var plot in plots)
        //        {
        //            if (plot.Points == null)
        //            {
        //                continue;
        //            }
        //            var points = plot.Points;

        //            if (points.Count() <= 1)
        //            {
        //                continue;
        //            }
        //            var s = xAxis.ToPixels(points.First().X);
        //            var e = xAxis.ToPixels(points.Last().X);
        //            var w = Math.Abs(e - s) / (points.Count() - 1);

        //            if (double.IsNaN(retBarWidth))
        //            {
        //                retBarWidth = w;
        //            }
        //            else
        //            {
        //                retBarWidth = w < retBarWidth ? w : retBarWidth;
        //            }
        //        }

        //        return retBarWidth;
        //    }
        //    catch
        //    {
        //        return double.NaN;
        //    }
        //}

        //protected virtual void Render(DrawingContext dc, DataSeries series, XAxis xAxis, YAxis yAxis, double barWidth, double offset)
        //{
        //    if (xAxis == null)
        //    {
        //        return;
        //    }

        //    IEnumerable<Plot> plots = xAxis.ToPlot(series.Points);

        //    foreach (var plot in plots)
        //    {
        //        Render(dc, series, plot, xAxis, yAxis, barWidth, offset);
        //    }
        //}

        //private void Render(DrawingContext dc, DataSeries series, Plot plot, XAxis xAxis, YAxis yAxis, double barWidth, double offset)
        //{
        //    if (plot == null || plot.Points == null)
        //    {
        //        return;
        //    }

        //    // Curve point marker drawing.
        //    IPointMarker iPointMarker = series as IPointMarker;
        //    Debug.Assert(iPointMarker != null, "iPointMarker != null");

        //    var xlong = xAxis.StopPos;
        //    var ylong = yAxis.StopPos;

        //    // Clipping region
        //    RectangleGeometry clip;
        //    if (series.Orientation == Orientation.HORIZONTAL)
        //        clip = new RectangleGeometry(new Rect(0, 0, xlong, ylong));
        //    else
        //        clip = new RectangleGeometry(new Rect(0, 0, ylong, xlong));
        //    dc.PushClip(clip);

        //    //bool flag = true;
        //    foreach (var pt in plot.Points)
        //    {
        //        double x, y; // coordinates in pixels
        //        bool isPtInsideArea;
        //        try
        //        {
        //            if (series.Orientation == Orientation.HORIZONTAL)
        //            {
        //                x = xAxis.ToPixels(pt.X);
        //                y = yAxis.ToPixels(pt.Y);
        //                //System.Diagnostics.Trace.WriteLine(string.Format("Horizontal XY : Point ({0}, {1})", x, y));
        //                isPtInsideArea = isInsideArea(new Point(x, y), new Size(xlong, ylong));
        //            }
        //            else // Orientation == Orientation.Vertical
        //            {
        //                y = xAxis.ToPixels(pt.X);
        //                x = yAxis.ToPixels(pt.Y);
        //                //if (flag)
        //                {
        //                    //System.Diagnostics.Trace.WriteLine(string.Format("Vertical XY : Point ({0}, {1})", x, y));
        //                    //flag = false;
        //                }
        //                isPtInsideArea = isInsideArea(new Point(x, y), new Size(ylong, xlong));
        //            }
        //        }
        //        catch (ArgumentException)
        //        {
        //            continue;
        //        }

        //        Brush columnBrush = series.Pen.Brush;

        //        // Draw the column.
        //        StreamGeometry geometry = new StreamGeometry();
        //        using (StreamGeometryContext ctx = geometry.Open())
        //        {
        //            if (series.Orientation == Orientation.HORIZONTAL)
        //            {
        //                double xStart = x + offset;
        //                double xStop = xStart + barWidth;
        //                //System.Diagnostics.Trace.WriteLine(string.Format("Horizontal (xStart, xStop) : ({0}, {1})    offset = {2}", xStart, xStop, offset));
        //                ctx.BeginFigure(new Point(xStart, ylong)
        //                    , true /* is filled */, true /* is closed */);
        //                ctx.PolyLineTo(new Point[] 
        //                { 
        //                    new Point(xStart, y), 
        //                    new Point(xStop, y),
        //                    new Point(xStop, ylong)
        //                }, false /* is stroked */, false /* is smooth join */);
        //            }
        //            else
        //            {
        //                double yStart = y - offset;
        //                double yStop = yStart - barWidth;
        //                //System.Diagnostics.Trace.WriteLine(string.Format("Vertical (yStart, yStop) : ({0}, {1})    offset = {2}", yStart, yStop, offset));
        //                ctx.BeginFigure(new Point(0, yStart)
        //                    , true /* is filled */, true /* is closed */);
        //                ctx.PolyLineTo(new Point[] 
        //                { 
        //                    new Point(x, yStart), 
        //                    new Point(x, yStop),
        //                    new Point(0, yStop)
        //                }, false /* is stroked */, false /* is smooth join */);
        //            }
        //        }
        //        geometry.Freeze();
        //        dc.DrawGeometry(columnBrush, null, geometry);

        //        // KeyPoints Marker
        //        Drawing pointMarker = pt.Emphasis ? iPointMarker.EmphasisPointMarker : null;
        //        DrawPointsMarker(pointMarker, x, y);
        //    }
        //}
    }
}
