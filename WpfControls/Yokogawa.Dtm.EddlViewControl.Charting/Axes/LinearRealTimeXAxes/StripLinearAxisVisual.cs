using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class StripLinearAxisVisual : AxisVisual, IDrawingContextProvider
    {
        public StripLinearXAxis StripLinearAxis
        {
            get
            {
                return Axis as StripLinearXAxis;
            }
        }

        private DefaultAxisVisualModel defaultAxisModel = new DefaultAxisVisualModel();
        private DefaultAxisVisualModel GetAxisModel()
        {
            defaultAxisModel.FontFamily = this.FontFamily;

            defaultAxisModel.FontSize = this.FontSize;

            defaultAxisModel.FontWeight = this.FontWeight;

            defaultAxisModel.FontStyle = this.FontStyle;

            defaultAxisModel.FontStretch = this.FontStretch;

            defaultAxisModel.LabelBrush = StripLinearAxis.TickLabelPen.Brush;

            defaultAxisModel.TitleFontSize = StripLinearAxis.TitleFontSize;

            defaultAxisModel.TitleBrush = StripLinearAxis.TitlePen.Brush;

            defaultAxisModel.Orientation = StripLinearAxis.Orientation;

            defaultAxisModel.ShowTitle = StripLinearAxis.ShowTitle;

            defaultAxisModel.Title = StripLinearAxis.Title;

            defaultAxisModel.ContentLayout = this.ContentLayout;

            defaultAxisModel.TickPen = StripLinearAxis.TickPen;

            defaultAxisModel.TickBaseLinePen = StripLinearAxis.TickBaseLinePen;

            defaultAxisModel.LabelMargin = 2.0;

            defaultAxisModel.TitleMargin = 2.0;

            defaultAxisModel.PlotLength = StripLinearAxis.Extent;

            defaultAxisModel.PlotStartPos = StripLinearAxis.StartPixelsPos;

            defaultAxisModel.PlotStopPos = StripLinearAxis.StopPixelsPos;

            defaultAxisModel.DataConverter = StripLinearAxis.DataConverter;

            defaultAxisModel.CustomFormatter = StripLinearAxis.CustomFormatter;

            defaultAxisModel.TitleCustomFormatter = StripLinearAxis.TitleCustomFormatter;

            defaultAxisModel.MinMajorTickInterval = StripLinearAxis.MinMajorTickInterval;

            defaultAxisModel.MaxMajorTickInterval = StripLinearAxis.MaxMajorTickInterval;

            return defaultAxisModel;
        }

        private DefaultLinearAxisVisualModel defaultLinearAxisModel = new DefaultLinearAxisVisualModel();
        private DefaultLinearAxisVisualModel GetLinearAxisModel()
        {
            defaultLinearAxisModel.ShowMajorTick = StripLinearAxis.ShowMajorTick;

            defaultLinearAxisModel.ShowMinorTick = StripLinearAxis.ShowMinorTick;

            defaultLinearAxisModel.MajorTickLength = StripLinearAxis.MajorTickLength;

            defaultLinearAxisModel.MinorTickLength = StripLinearAxis.MinorTickLength;

            defaultLinearAxisModel.Minimum = StripLinearAxis.PrePartScale.Minimum;

            defaultLinearAxisModel.Maximum = StripLinearAxis.PrePartScale.Maximum;

            return defaultLinearAxisModel;
        }

        private LinearAxisVisualGraph linearAxisVisualGraph = null;
        public override void Render(AxisVisualContext context)
        {
            Children.Clear();

            if (linearAxisVisualGraph == null)
            {
                linearAxisVisualGraph = new LinearAxisVisualGraph();
            }
            linearAxisVisualGraph.AxisModel = GetAxisModel();
            linearAxisVisualGraph.LinearAxisModel = GetLinearAxisModel();
            if (StripLinearAxis.IsVisible)
            {
                linearAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                StripLinearAxis.SetGridLines(linearAxisVisualGraph.GetGridLines());
            }
            else
            {
                using (DrawingContext dc = RenderOpen())
                {
                }
                StripLinearAxis.SetGridLines(null);
            }
        }

        public override Rect GetRenderBound()
        {
            if (StripLinearAxis.IsVisible && linearAxisVisualGraph != null)
            {
                return linearAxisVisualGraph.GetRenderBound();
            }
            return new Rect(0.0, 0.0, 0.0, 0.0);
        }

        public DrawingContext AxisRenderOpen()
        {
            return this.RenderOpen();
        }
    }
}
