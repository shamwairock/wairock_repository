using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DataSeriesVisual : DrawingVisual
    {
        #region DataSeries
        public DataSeries DataSeries
        {
            get { return (DataSeries)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }

        public static readonly DependencyProperty DataSeriesProperty = DataSeriesView.DataSeriesProperty.AddOwner(typeof(DataSeriesVisual));

        #endregion

        protected static bool isInsideArea(Point pt, Size area)
        {
            System.Diagnostics.Debug.Assert(area.Width >= 0, "area.Width >= 0");
            System.Diagnostics.Debug.Assert(area.Height >= 0, "area.Height >= 0");

            if (pt.X < 0 || pt.X > area.Width || pt.Y < 0 || pt.Y > area.Height)
            {
                return false;
            }

            return true;
        }

        public virtual void Render(VisualContext context)
        {
            Children.Clear();

            if (context == null)
            {
                return;
            }

            var xAxis = this.DataSeries.XAxis;
            var yAxis = this.DataSeries.YAxis;
            var sources = context.Sources;
            if (this.DataSeries != null && xAxis != null && yAxis != null && sources != null)
            {
                using (DrawingContext dc = RenderOpen())
                {
                    if (this.DataSeries.IsVisible)
                    {
                        Render(dc, this.DataSeries, this.DataSeries.XAxis, this.DataSeries.YAxis, context.Sources);
                    }
                }
            }
        }

        protected virtual void Render(DrawingContext dc, DataSeries series, XAxis xAxis, YAxis yAxis, IList<DataSeries> sources)
        {
            IEnumerable<Plot> plots = xAxis.ToPlot(series.Points);
            if (plots != null)
            {
                foreach (var plot in plots)
                {
                    if (plot.Points != null)
                    {
                        Render(dc, series, plot.Points, xAxis, yAxis, sources);
                    }
                }
            }
        }

        protected virtual void Render(DrawingContext dc, DataSeries series, IEnumerable<IDataPoint> points, XAxis xAxis, YAxis yAxis, IList<DataSeries> sources) 
        { 
        }

        protected void DrawPointsMarker(Drawing markerDrawing, double x, double y)
        {
            if (markerDrawing == null)
            {
                return;
            }

            // Curve point marker.
            Drawing marker = markerDrawing.Clone();
            marker.Freeze();

            DataPointVisual pointMarker = new DataPointVisual(marker);
            pointMarker.Transform = new TranslateTransform(x, y);
            Children.Add(pointMarker);
        }
    }
}
