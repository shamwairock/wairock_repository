using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class MultiAxesGrid : ContentControl, IUpdate
    {
        public static readonly DependencyProperty AxesProperty
           = DependencyProperty.Register("Axes", typeof(IList<Axis>), typeof(MultiAxesGrid)
               , new FrameworkPropertyMetadata(null, 
                   FrameworkPropertyMetadataOptions.AffectsMeasure| 
                   FrameworkPropertyMetadataOptions.AffectsRender, 
                   AxesPropertyChanged));

        public IList<Axis> Axes
        {
            get { return (IList<Axis>)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty
            = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(MultiAxesGrid), 
                new PropertyMetadata(Orientation.VERTICAL));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty GridVisibilityProperty
            = DependencyProperty.Register("GridVisibility", typeof(GridVisibility), typeof(MultiAxesGrid), 
                new PropertyMetadata(GridVisibility.ALLTICKS));

        public GridVisibility GridVisibility
        {
            get { return (GridVisibility)GetValue(GridVisibilityProperty); }
            set { SetValue(GridVisibilityProperty, value); }
        }

        #region Horizontal HorizontalPen
        public static readonly DependencyProperty HorizontalPenProperty
           = DependencyProperty.RegisterAttached("HorizontalPen", typeof(Pen), typeof(MultiAxesGrid), 
               new FrameworkPropertyMetadata(new Pen(Brushes.WhiteSmoke, 0.5) { DashStyle = new DashStyle(new double[] { 2, 4 }, 0) }, 
                   FrameworkPropertyMetadataOptions.AffectsMeasure | 
                   FrameworkPropertyMetadataOptions.AffectsRender  | 
                   FrameworkPropertyMetadataOptions.Inherits));

        public Pen HorizontalPen
        {
            get { return (Pen)GetValue(HorizontalPenProperty); }
            set { SetValue(HorizontalPenProperty, value); }
        }

        public static Pen GetHorizontalPen(DependencyObject element)
        {
            return (Pen)element.GetValue(HorizontalPenProperty);
        }

        public static void SetHorizontalPen(DependencyObject element, Pen value)
        {
            element.SetValue(HorizontalPenProperty, value);
        }
        #endregion

        #region Vertical HorizontalPen

        public static readonly DependencyProperty VerticalPenProperty =
            DependencyProperty.RegisterAttached("VerticalPen", typeof(Pen), typeof(MultiAxesGrid),
                 new FrameworkPropertyMetadata(new Pen(Brushes.WhiteSmoke, 0.5) { DashStyle = new DashStyle(new double[] { 2, 4 }, 0) },
                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                   FrameworkPropertyMetadataOptions.AffectsRender |
                   FrameworkPropertyMetadataOptions.Inherits));

        public Pen VerticalPen
        {
            get { return (Pen)GetValue(VerticalPenProperty); }
            set { SetValue(VerticalPenProperty, value); }
        }

        public static Pen GetVerticalPen(DependencyObject element)
        {
            return (Pen)element.GetValue(VerticalPenProperty);
        }

        public static void SetVerticalPen(DependencyObject element, Pen value)
        {
            element.SetValue(VerticalPenProperty, value);
        }

        #endregion

        private static void AxesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MultiAxesGrid ths = sender as MultiAxesGrid;
            if (ths == null)
                return;

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged -= ths.Axes_CollectionChanged;
                }
                var axes = e.OldValue as IList<Axis>;
                {
                    if (axes != null && axes.Count > 0)
                    {
                        foreach (var axis in axes)
                        {
                            if (axis != null)
                            {
                                axis.PropertyChanged -= ths.ChartScalePropertyChanged;
                            }
                        }
                    }
                }
            }
            if (e.NewValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += ths.Axes_CollectionChanged;
                }
                var axes = e.NewValue as IList<Axis>;
                {
                    if (axes != null && axes.Count > 0)
                    {
                        foreach (var axis in axes)
                        {
                            if (axis != null)
                            {
                                axis.PropertyChanged += ths.ChartScalePropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        private void Axes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var s = item as Axis;
                    if (s != null)
                    {
                        s.PropertyChanged -= this.ChartScalePropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var s = item as DataSeries;
                    if (s != null)
                    {
                        s.PropertyChanged += this.ChartScalePropertyChanged;
                    }
                }
            }
        }

        private void ChartScalePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowGridLine" || e.PropertyName == "GridLinePen")
            {
                Update();
            }
        }

        

        private double gridBrushThickness = 1;
        private Path path = new Path();
        private Canvas canvas = new Canvas();

        public MultiAxesGrid()
        {
            IsHitTestVisible = false;

            canvas.ClipToBounds = true;

            path.Stroke = Brushes.LightGray;
            path.StrokeThickness = gridBrushThickness;

            Content = canvas;
        }

        private bool updating = false;
        public void Update()
        {
            InvalidateVisual();
            InvalidateMeasure();

            if (updating)
            {
                return;
            }
            updating = true;

            canvas.Children.Clear();
            if (this.Axes != null && this.Axes.Count > 0)
            {
                foreach (var axis in this.Axes)
                {
                    DrawGrid(axis, canvas.Children, this.RenderSize);
                }
            }
            
            updating = false;
        }

        private void DrawGrid(Axis axis, UIElementCollection uIElementCollection, Size size)
        {
            var gridProvider = axis as IGridLineProvider;
            if (gridProvider == null || axis == null || HorizontalPen == null || GridVisibility == GridVisibility.HIDDEN)
            {
                return; // Nothing to draw
            }

            if (!axis.ShowGridLine)
            {
                return;// Nothing to draw
            }

            var gridLines = gridProvider.GetGridLines();
            if(gridLines == null || gridLines.Length <= 0)
            {
                return; // Nothing to draw
            }

            if (Orientation == Orientation.VERTICAL)
            {
                var gridLineLength = size.Height;
                var gridPen = axis.GridLinePen != null ? axis.GridLinePen : this.VerticalPen;
                foreach (var line in gridLines)
                {
                    if (!line.IsLong && GridVisibility == GridVisibility.LONGTICKS)
                    {
                        continue;
                    }
                    uIElementCollection.Add(CreateLine(line.Position, 0, line.Position, gridLineLength, gridPen));
                }
            }
            else
            {
                double gridLineLength = size.Width;
                var gridPen = axis.GridLinePen != null ? axis.GridLinePen : this.HorizontalPen;
                foreach (var line in gridLines)
                {
                    if (!line.IsLong && GridVisibility == GridVisibility.LONGTICKS)
                    {
                        continue;
                    }
                    uIElementCollection.Add(CreateLine(0, line.Position, gridLineLength, line.Position, gridPen));
                }
            }
        }

        private Line CreateLine(double x1, double y1, double x2, double y2, Pen linePen)
        {
            DebugTraceLog.WriteLine(string.Format("x1={0},y1={1},x2={2},y2={3}", x1, y1, x2, y2));

            linePen = linePen!= null ? linePen : new Pen(Brushes.Red, 1);
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
