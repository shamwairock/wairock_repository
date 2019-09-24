using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DataSeriesView : FrameworkElement, ISeriesRender
    {
        protected VisualCollection visuals;

        public DataSeriesView()
        {
            this.SnapsToDevicePixels = true;
            visuals = new VisualCollection(this);
        }

        public DataSeries DataSeries
        {
            get { return (DataSeries)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }

        public static readonly DependencyProperty DataSeriesProperty
            = DependencyProperty.Register("DataSeries", typeof(DataSeries), typeof(DataSeriesView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, ChartSourcePropertyChanged));

        private static void ChartSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //DataSeriesView srcView = sender as DataSeriesView;
            //if (srcView != null)
            //{
            //    DataSeries oldItem = e.OldValue as DataSeries;
            //    if (oldItem != null)
            //    {
            //        oldItem.PropertyChanged -= srcView.ChartSourceChanged;
            //    }

            //    DataSeries newItem = e.NewValue as DataSeries;
            //    if (newItem != null)
            //    {
            //        newItem.PropertyChanged += srcView.ChartSourceChanged;
            //    }
            //}
        }

        private void ChartSourceChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChartSourceChanged(e);

            if (e.PropertyName == "VisualCue")
            {
                RaiseEvent(new RoutedEventArgs(VisualCueChangedEvent));
            }
            if (e.PropertyName == "IsVisible")
            {

            }
            RenderVisuals(null);
            InvalidateVisual();
            InvalidateMeasure();
        }

        protected virtual void OnChartSourceChanged(PropertyChangedEventArgs e) { }

        public static readonly RoutedEvent VisualCueChangedEvent
            = EventManager.RegisterRoutedEvent("VisualCueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataSeriesView));

        public event RoutedEventHandler VisualCueChanged
        {
            add { AddHandler(VisualCueChangedEvent, value); }
            remove { RemoveHandler(VisualCueChangedEvent, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            RenderVisuals(null);
        }

        private void RenderVisuals(VisualContext context)
        {
            foreach (Visual visual in visuals)
            {
                DataSeriesVisual sourceVisual = visual as DataSeriesVisual;
                System.Diagnostics.Debug.Assert(sourceVisual != null, "sourceVisual != null");
                sourceVisual.Render(context);
            }
        }

        private object ItemHitTest(Point pt)
        {
            foreach (Visual visual in visuals)
            {
                DataSeriesVisual sourceVisual = visual as DataSeriesVisual;
                System.Diagnostics.Debug.Assert(sourceVisual != null, "sourceVisual != null");
                if (sourceVisual != null)
                {
                    HitTestResult result = VisualTreeHelper.HitTest(sourceVisual, pt);

                    if (result != null && (result.VisualHit is DataSeriesVisual || result.VisualHit is DataPointVisual))
                    {
                        return result.VisualHit;
                    }
                }
            }
            return null;
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index >= visuals.Count)
                throw new ArgumentOutOfRangeException("index");

            return visuals[index];
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Rect rect = Rect.Empty;
            foreach (Visual visual in visuals)
            {
                DataSeriesVisual sourceVisual = visual as DataSeriesVisual;
                System.Diagnostics.Debug.Assert(sourceVisual != null, "sourceVisual != null");

                rect.Union(sourceVisual.ContentBounds);
                rect.Union(sourceVisual.DescendantBounds);
            }

            if (rect.IsEmpty || double.IsInfinity(rect.Width) || double.IsInfinity(rect.Height))
            {
                return new Size(0, 0);
            }

            return new Size(rect.Width, rect.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        public void Render(VisualContext context)
        {
            RenderVisuals(context);
            InvalidateVisual();
            InvalidateMeasure();
        }
    }
}
