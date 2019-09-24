using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearAxisVisual : AxisVisual, IDrawingContextProvider
    {
        public LinearAxis LinearAxis
        {
            get
            {
                return Axis as LinearAxis;
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

            defaultAxisModel.LabelBrush = LinearAxis.TickLabelPen.Brush;

            defaultAxisModel.TitleFontSize = LinearAxis.TitleFontSize;

            defaultAxisModel.TitleBrush = LinearAxis.TitlePen.Brush;

            defaultAxisModel.Orientation = LinearAxis.Orientation;

            defaultAxisModel.ShowTitle = LinearAxis.ShowTitle;

            defaultAxisModel.Title = LinearAxis.Title;

            defaultAxisModel.ContentLayout = this.ContentLayout;

            defaultAxisModel.TickPen = LinearAxis.TickPen;

            defaultAxisModel.TickBaseLinePen = LinearAxis.TickBaseLinePen;

            defaultAxisModel.LabelMargin = 2.0;

            defaultAxisModel.TitleMargin = 10.0;

            defaultAxisModel.PlotLength = LinearAxis.Extent;

            defaultAxisModel.PlotStartPos = LinearAxis.StartPixelsPos;

            defaultAxisModel.PlotStopPos = LinearAxis.StopPixelsPos;

            defaultAxisModel.DataConverter = LinearAxis.DataConverter; 
            
            defaultAxisModel.CustomFormatter = LinearAxis.CustomFormatter;

            defaultAxisModel.TitleCustomFormatter = LinearAxis.TitleCustomFormatter;

            defaultAxisModel.MinMajorTickInterval = LinearAxis.MinMajorTickInterval;

            defaultAxisModel.MaxMajorTickInterval = LinearAxis.MaxMajorTickInterval;

            return defaultAxisModel;
        }

        private DefaultLinearAxisVisualModel defaultLinearAxisModel = new DefaultLinearAxisVisualModel();
        private DefaultLinearAxisVisualModel GetLinearAxisModel()
        {
            defaultLinearAxisModel.ShowMajorTick = LinearAxis.ShowMajorTick;

            defaultLinearAxisModel.ShowMinorTick = LinearAxis.ShowMinorTick;

            defaultLinearAxisModel.MajorTickLength = LinearAxis.MajorTickLength;

            defaultLinearAxisModel.MinorTickLength = LinearAxis.MinorTickLength;

            defaultLinearAxisModel.Minimum = LinearAxis.Scale.Minimum;

            defaultLinearAxisModel.Maximum = LinearAxis.Scale.Maximum;

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
            if (LinearAxis.IsVisible)
            {
                linearAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                LinearAxis.SetGridLines(linearAxisVisualGraph.GetGridLines());
            }
            else
            {
                using (DrawingContext dc = RenderOpen())
                {
                }
                LinearAxis.SetGridLines(null);
            }
        }

        public override Rect GetRenderBound()
        {
            if (LinearAxis.IsVisible && linearAxisVisualGraph != null)
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
