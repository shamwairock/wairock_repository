using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class SweepLinearAxisVisual : AxisVisual, IDrawingContextProvider
    {
        public SweepLinearXAxis SweepLinearAxis
        {
            get
            {
                return Axis as SweepLinearXAxis;
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

            defaultAxisModel.LabelBrush = SweepLinearAxis.TickLabelPen.Brush;

            defaultAxisModel.TitleFontSize = SweepLinearAxis.TitleFontSize;

            defaultAxisModel.TitleBrush = SweepLinearAxis.TitlePen.Brush;

            defaultAxisModel.Orientation = SweepLinearAxis.Orientation;

            defaultAxisModel.ShowTitle = SweepLinearAxis.ShowTitle;

            defaultAxisModel.Title = SweepLinearAxis.Title;

            defaultAxisModel.ContentLayout = this.ContentLayout;

            defaultAxisModel.TickPen = SweepLinearAxis.TickPen;

            defaultAxisModel.TickBaseLinePen = SweepLinearAxis.TickBaseLinePen;

            defaultAxisModel.LabelMargin = 2.0;

            defaultAxisModel.TitleMargin = 2.0;

            defaultAxisModel.PlotLength = SweepLinearAxis.Extent;

            defaultAxisModel.PlotStartPos = SweepLinearAxis.StartPixelsPos;

            defaultAxisModel.PlotStopPos = SweepLinearAxis.StopPixelsPos;

            defaultAxisModel.DataConverter = SweepLinearAxis.DataConverter;

            defaultAxisModel.CustomFormatter = SweepLinearAxis.CustomFormatter;

            defaultAxisModel.TitleCustomFormatter = SweepLinearAxis.TitleCustomFormatter;

            defaultAxisModel.MinMajorTickInterval = SweepLinearAxis.MinMajorTickInterval;

            defaultAxisModel.MaxMajorTickInterval = SweepLinearAxis.MaxMajorTickInterval;

            return defaultAxisModel;
        }

        private DefaultSweepLinearAxisVisualModel defaultSweepLinearAxisModel = new DefaultSweepLinearAxisVisualModel();
        private DefaultSweepLinearAxisVisualModel GetSweepLinearAxisModel()
        {
            defaultSweepLinearAxisModel.ShowMajorTick = SweepLinearAxis.ShowMajorTick;

            defaultSweepLinearAxisModel.ShowMinorTick = SweepLinearAxis.ShowMinorTick;

            defaultSweepLinearAxisModel.MajorTickLength = SweepLinearAxis.MajorTickLength;

            defaultSweepLinearAxisModel.MinorTickLength = SweepLinearAxis.MinorTickLength;

            defaultSweepLinearAxisModel.PreMinimum = SweepLinearAxis.PrePartScale.Minimum;

            defaultSweepLinearAxisModel.PreMaximum = SweepLinearAxis.PrePartScale.Maximum;

            defaultSweepLinearAxisModel.PostMinimum = SweepLinearAxis.PostPartScale.Minimum;

            defaultSweepLinearAxisModel.PostMaximum = SweepLinearAxis.PostPartScale.Maximum;

            defaultSweepLinearAxisModel.PostPartValid = SweepLinearAxis.PostPartValid;

            return defaultSweepLinearAxisModel;
        }

        private SweepLinearAxisVisualGraph sweepLinearAxisVisualGraph = null;
        public override void Render(AxisVisualContext context)
        {
            Children.Clear();

            if (sweepLinearAxisVisualGraph == null)
            {
                sweepLinearAxisVisualGraph = new SweepLinearAxisVisualGraph();
            }
            sweepLinearAxisVisualGraph.SweepLinearXAxis = SweepLinearAxis;
            sweepLinearAxisVisualGraph.AxisModel = GetAxisModel();
            sweepLinearAxisVisualGraph.SweepLinearAxisModel = GetSweepLinearAxisModel();
            if (SweepLinearAxis.IsVisible)
            {
                sweepLinearAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                SweepLinearAxis.SetGridLines(sweepLinearAxisVisualGraph.GetGridLines());
            }
            else
            {
                using (DrawingContext dc = RenderOpen())
                {
                }
                SweepLinearAxis.SetGridLines(null);
            }
        }

        public override Rect GetRenderBound()
        {
            if (SweepLinearAxis.IsVisible && sweepLinearAxisVisualGraph != null)
            {
                return sweepLinearAxisVisualGraph.GetRenderBound();
            }
            return new Rect(0.0, 0.0, 0.0, 0.0);
        }

        public DrawingContext AxisRenderOpen()
        {
            return this.RenderOpen();
        }
    }
}
