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

        protected bool isInsideArea(double x, double y, double areaWidth, double areaHeight)
        {
            System.Diagnostics.Debug.Assert(areaWidth >= 0, "area.Width >= 0");
            System.Diagnostics.Debug.Assert(areaHeight >= 0, "area.Height >= 0");

            if (x < 0 || x > areaWidth || y < 0 || y > areaHeight)
            {
                return false;
            }
            return true;
        }

        public virtual void Render(VisualContext context)
        {
            Children.Clear();

            var reDrawContext = false;

            var xAxis = this.DataSeries.XAxis;
            var yAxis = this.DataSeries.YAxis;
            var sources = context != null ? context.Sources : null;
            if (this.DataSeries != null && this.DataSeries.IsVisible && xAxis != null && yAxis != null /*&& sources != null*/)
            {
                using (DrawingContext dc = RenderOpen())
                {
                    reDrawContext = true;
                    if (this.DataSeries.IsVisible)
                    {
                        Render(dc, this.DataSeries, this.DataSeries.XAxis, this.DataSeries.YAxis, this.DataSeries.PlotFlowAxisOption, sources);
                    }
                }
            }

            if (reDrawContext == false)
            {
                using (DrawingContext dc = RenderOpen())
                {
                }
            }
        }

        protected virtual void Render(DrawingContext dc, DataSeries series, Axis xAxis, Axis yAxis, PlotFlowAxisOptions plotFlowAxisOption, IList<DataSeries> sources)
        {
            IEnumerable<Plot> plots = null;
            if(plotFlowAxisOption == PlotFlowAxisOptions.XAxis)
            {
                plots = xAxis.ToPlot(series.Points);
            }
            else
            {
                plots = yAxis.ToPlot(series.Points);
            }
            if (plots != null)
            {
                foreach (var plot in plots)
                {
                    Render(dc, series, plot, xAxis, yAxis, sources);
                }
            }
        }

        protected virtual void Render(DrawingContext dc, DataSeries series, Plot plot, Axis xAxis, Axis yAxis, IList<DataSeries> sources)
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
