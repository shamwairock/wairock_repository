using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicAxisVisual : AxisVisual, IDrawingContextProvider
    {
        public LogarithmicAxis LogarithmicAxis
        {
            get
            {
                return Axis as LogarithmicAxis;
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

            defaultAxisModel.LabelBrush = LogarithmicAxis.TickLabelPen.Brush;

            defaultAxisModel.TitleFontSize = LogarithmicAxis.TitleFontSize;

            defaultAxisModel.TitleBrush = LogarithmicAxis.TitlePen.Brush;

            defaultAxisModel.Orientation = LogarithmicAxis.Orientation;

            defaultAxisModel.ShowTitle = LogarithmicAxis.ShowTitle;

            defaultAxisModel.Title = LogarithmicAxis.Title;

            defaultAxisModel.ContentLayout = this.ContentLayout;

            defaultAxisModel.TickPen = LogarithmicAxis.TickPen;

            defaultAxisModel.TickBaseLinePen = LogarithmicAxis.TickBaseLinePen;

            defaultAxisModel.LabelMargin = 2.0;

            defaultAxisModel.TitleMargin = 10.0;

            defaultAxisModel.PlotLength = LogarithmicAxis.Extent;

            defaultAxisModel.PlotStartPos = LogarithmicAxis.StartPixelsPos;

            defaultAxisModel.PlotStopPos = LogarithmicAxis.StopPixelsPos;

            defaultAxisModel.DataConverter = LogarithmicAxis.DataConverter;

            defaultAxisModel.CustomFormatter = LogarithmicAxis.CustomFormatter;

            defaultAxisModel.TitleCustomFormatter = LogarithmicAxis.TitleCustomFormatter;

            defaultAxisModel.MinMajorTickInterval = LogarithmicAxis.MinMajorTickInterval;

            defaultAxisModel.MaxMajorTickInterval = LogarithmicAxis.MaxMajorTickInterval;

            return defaultAxisModel;
        }

        private DefaultLogarithmicAxisVisualModel defaultLogarithmicAxisModel = new DefaultLogarithmicAxisVisualModel();
        private DefaultLogarithmicAxisVisualModel GetLogarithmicAxisModel()
        {
            defaultLogarithmicAxisModel.ShowMajorTick = LogarithmicAxis.ShowMajorTick;

            defaultLogarithmicAxisModel.ShowMinorTick = LogarithmicAxis.ShowMinorTick;

            defaultLogarithmicAxisModel.MajorTickLength = LogarithmicAxis.MajorTickLength;

            defaultLogarithmicAxisModel.MinorTickLength = LogarithmicAxis.MinorTickLength;

            defaultLogarithmicAxisModel.Minimum = LogarithmicAxis.LogScale.Minimum;

            defaultLogarithmicAxisModel.Maximum = LogarithmicAxis.LogScale.Maximum;

            return defaultLogarithmicAxisModel;
        }

        private DefaultLinearAxisVisualModel defaultLinearAxisModel = new DefaultLinearAxisVisualModel();
        private DefaultLinearAxisVisualModel GetLinearAxisModel()
        {
            defaultLinearAxisModel.ShowMajorTick = LogarithmicAxis.ShowMajorTick;

            defaultLinearAxisModel.ShowMinorTick = LogarithmicAxis.ShowMinorTick;

            defaultLinearAxisModel.MajorTickLength = LogarithmicAxis.MajorTickLength;

            defaultLinearAxisModel.MinorTickLength = LogarithmicAxis.MinorTickLength;

            defaultLinearAxisModel.Minimum = LogarithmicAxis.LinearScale.Minimum;

            defaultLinearAxisModel.Maximum = LogarithmicAxis.LinearScale.Maximum;

            return defaultLinearAxisModel;
        }

        private AxisVisualGraph currentAxis = null;

        private LogarithmicAxisVisualGraph logarithmicAxisVisualGraph = null;
        private LinearAxisVisualGraph linearAxisVisualGraph = null;
        public override void Render(AxisVisualContext context)
        {
            Children.Clear();

            if (logarithmicAxisVisualGraph == null)
            {
                logarithmicAxisVisualGraph = new LogarithmicAxisVisualGraph();
            }
            if (linearAxisVisualGraph == null)
            {
                linearAxisVisualGraph = new LinearAxisVisualGraph();
            }
            bool useLog = LogarithmicTickCreator.CanUseLogarithmicTick(new Range<double>(LogarithmicAxis.LogScale.Minimum, LogarithmicAxis.LogScale.Maximum));
            if (useLog)
            {
                LogarithmicAxis.ShowLogAxis = true;
                logarithmicAxisVisualGraph.AxisModel = GetAxisModel();
                logarithmicAxisVisualGraph.LogarithmicAxixModel = GetLogarithmicAxisModel();
                if (LogarithmicAxis.IsVisible)
                {
                    logarithmicAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                    LogarithmicAxis.SetGridLines(logarithmicAxisVisualGraph.GetGridLines());
                }
                else
                {
                    using (DrawingContext dc = RenderOpen())
                    {
                    }
                    LogarithmicAxis.SetGridLines(null);
                }
                currentAxis = logarithmicAxisVisualGraph;
            }
            else
            {
                LogarithmicAxis.ShowLogAxis = false;
                linearAxisVisualGraph.AxisModel = GetAxisModel();
                linearAxisVisualGraph.LinearAxisModel = GetLinearAxisModel();
                if (LogarithmicAxis.IsVisible)
                {
                    linearAxisVisualGraph.Render(new AxisVisualContext() { Render = this });
                    LogarithmicAxis.SetGridLines(linearAxisVisualGraph.GetGridLines());
                }
                else
                {
                    using (DrawingContext dc = RenderOpen())
                    {
                    }
                    LogarithmicAxis.SetGridLines(null);
                }
                currentAxis = linearAxisVisualGraph;
            }
        }

        public override Rect GetRenderBound()
        {
            if (LogarithmicAxis.IsVisible && currentAxis != null)
            {
                return currentAxis.GetRenderBound();
            }
            return new Rect(0.0, 0.0, 0.0, 0.0);
        }

        public DrawingContext AxisRenderOpen()
        {
            return RenderOpen();
        }
    }
}
