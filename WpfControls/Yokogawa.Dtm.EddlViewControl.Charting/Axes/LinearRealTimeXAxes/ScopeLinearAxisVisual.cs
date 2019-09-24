using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ScopeLinearAxisVisual : AxisVisual, IDrawingContextProvider
    {
        public ScopeLinearXAxis ScopeLinearAxis
        {
            get
            {
                return Axis as ScopeLinearXAxis;
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

            defaultAxisModel.LabelBrush = ScopeLinearAxis.TickLabelPen.Brush;

            defaultAxisModel.TitleFontSize = ScopeLinearAxis.TitleFontSize;

            defaultAxisModel.TitleBrush = ScopeLinearAxis.TitlePen.Brush;

            defaultAxisModel.Orientation = ScopeLinearAxis.Orientation;

            defaultAxisModel.ShowTitle = ScopeLinearAxis.ShowTitle;

            defaultAxisModel.Title = ScopeLinearAxis.Title;

            defaultAxisModel.ContentLayout = this.ContentLayout;

            defaultAxisModel.TickPen = ScopeLinearAxis.TickPen;

            defaultAxisModel.TickBaseLinePen = ScopeLinearAxis.TickBaseLinePen;

            defaultAxisModel.LabelMargin = 2.0;

            defaultAxisModel.TitleMargin = 2.0;

            defaultAxisModel.PlotLength = ScopeLinearAxis.Extent;

            defaultAxisModel.PlotStartPos = ScopeLinearAxis.StartPixelsPos;

            defaultAxisModel.PlotStopPos = ScopeLinearAxis.StopPixelsPos;

            defaultAxisModel.DataConverter = ScopeLinearAxis.DataConverter;

            defaultAxisModel.CustomFormatter = ScopeLinearAxis.CustomFormatter;

            defaultAxisModel.TitleCustomFormatter = ScopeLinearAxis.TitleCustomFormatter;

            defaultAxisModel.MinMajorTickInterval = ScopeLinearAxis.MinMajorTickInterval;

            defaultAxisModel.MaxMajorTickInterval = ScopeLinearAxis.MaxMajorTickInterval;

            return defaultAxisModel;
        }

        private DefaultLinearAxisVisualModel defaultLinearAxisModel = new DefaultLinearAxisVisualModel();
        private DefaultLinearAxisVisualModel GetLinearAxisModel()
        {
            defaultLinearAxisModel.ShowMajorTick = ScopeLinearAxis.ShowMajorTick;

            defaultLinearAxisModel.ShowMinorTick = ScopeLinearAxis.ShowMinorTick;

            defaultLinearAxisModel.MajorTickLength = ScopeLinearAxis.MajorTickLength;

            defaultLinearAxisModel.MinorTickLength = ScopeLinearAxis.MinorTickLength;

            defaultLinearAxisModel.Minimum = ScopeLinearAxis.PrePartScale.Minimum;

            defaultLinearAxisModel.Maximum = ScopeLinearAxis.PrePartScale.Maximum;

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
            if (ScopeLinearAxis.IsVisible)
            {
                linearAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                ScopeLinearAxis.SetGridLines(linearAxisVisualGraph.GetGridLines());
            }
            else
            {
                using (DrawingContext dc = RenderOpen())
                {
                }
                ScopeLinearAxis.SetGridLines(null);
            }
        }

        public override Rect GetRenderBound()
        {
            if (ScopeLinearAxis.IsVisible && linearAxisVisualGraph != null)
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
