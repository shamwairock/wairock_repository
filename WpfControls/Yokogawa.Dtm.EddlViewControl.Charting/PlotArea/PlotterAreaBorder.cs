using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class PlotterAreaBorder : ContentControl, IUpdate
    {
        private double gridBrushThickness = 1;
        private Path path = new Path() { SnapsToDevicePixels = true };
        private Canvas canvas = new Canvas();

        public PlotterAreaBorder()
        {
            IsHitTestVisible = false;
            path.Stroke = Brushes.LightGray;
            path.StrokeThickness = gridBrushThickness;
            Content = canvas;
        }

        private bool updating = false;
        public void Update()
        {
            if (updating)
            {
                return;
            }
            updating = true;

            canvas.Children.Clear();
            if (Visibility != Visibility.Visible)
            {
                return;
            }

            var size = RenderSize;
            canvas.Children.Add(CreateLine(0 - BorderThickness.Left / 2, 0, size.Width + BorderThickness.Left / 2, 0, new Pen(BorderBrush, BorderThickness.Left)));
            canvas.Children.Add(CreateLine(0 - BorderThickness.Right / 2, size.Height, size.Width + BorderThickness.Right / 2, size.Height, new Pen(BorderBrush, BorderThickness.Right)));

            canvas.Children.Add(CreateLine(0, 0, 0, size.Height, new Pen(BorderBrush, BorderThickness.Top)));
            canvas.Children.Add(CreateLine(size.Width, 0, size.Width, size.Height, new Pen(BorderBrush, BorderThickness.Bottom)));

            updating = false;
        }

        private Line CreateLine(double x1, double y1, double x2, double y2, Pen linePen)
        {
            linePen = (linePen != null) ? linePen : new Pen(Brushes.Black, 1);
            return new Line
            {
                Y1 = y1,
                Y2 = y2,
                X1 = x1,
                X2 = x2,
                Stroke = linePen.Brush,
                StrokeThickness = linePen.Thickness,
                StrokeDashCap = linePen.DashCap,
                StrokeDashArray = linePen.DashStyle != null ? linePen.DashStyle.Dashes : null,
                SnapsToDevicePixels = true,
            };
        }
    }
}
