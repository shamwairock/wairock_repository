using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class RangeSelectorAdorner : Adorner
    {
        private RangeSelectorAdornerCtrl adornerContol;
        public RangeSelectorAdornerCtrl AdornerContol
        {
            get { return adornerContol; }
            set { adornerContol = value; }
        }

        public RangeSelectorAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            adornerContol = new RangeSelectorAdornerCtrl();
            adornerContol.Visibility = System.Windows.Visibility.Visible;
            AddVisualChild(adornerContol);
        }

        protected override Visual GetVisualChild(int index)
        {
            return adornerContol;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        private double offsetH = -5;
        private double offsetV = 20;
        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            if (adornerContol != null && AdornedElement != null && AdornedElement.Visibility == Visibility.Visible)
            {
                adornerContol.Arrange(new Rect(new Point(-offsetH, -offsetV), new Point(finalSize.Width/* + offsetH*/, finalSize.Height + offsetV)));
            }
            adornerContol.Visibility = AdornedElement.Visibility;
            return size;
        }
    }

    public class RangeSelectorAdornerCtrl : Control
    {
        static RangeSelectorAdornerCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSelectorAdornerCtrl), new FrameworkPropertyMetadata(typeof(RangeSelectorAdornerCtrl)));
        }
    }

    public class RangeSelectorContainer : Control
    {
        static RangeSelectorContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSelectorContainer), new FrameworkPropertyMetadata(typeof(RangeSelectorContainer)));
        }

        public RangeSelectorContainer()
        {
            Loaded += RangeSelectorContainer_Loaded;
            Unloaded +=RangeSelectorContainer_Unloaded;
        }

        #region Properties
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(RangeSelectorContainer), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(RangeSelectorContainer), new PropertyMetadata(1.0));

        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register(
            "RangeStart", typeof(double), typeof(RangeSelectorContainer), new PropertyMetadata(0.0));

        public static readonly DependencyProperty RangeEndProperty = DependencyProperty.Register(
            "RangeEnd", typeof(double), typeof(RangeSelectorContainer), new PropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double RangeStart
        {
            get { return (double)GetValue(RangeStartProperty); }
            set { SetValue(RangeStartProperty, value); }
        }

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }

        #endregion

        private RangeSelectorAdorner adorner = null;
        private void RangeSelectorContainer_Loaded(object sender, RoutedEventArgs e)
        {
            DebugTraceLog.WriteLine("RangeSelectorContainer_Loaded");
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                if (adorner == null)
                {
                    adorner = new RangeSelectorAdorner(this);
                    adornerLayer.Add(adorner);
                }
                adorner.AdornerContol.DataContext = this;
            }
        }

        public void RangeSelectorContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            DebugTraceLog.WriteLine("RangeSelectorContainer_Unloaded");
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null && adorner != null)
            {
                if (adorner.AdornerContol != null)
                {
                    adorner.AdornerContol.DataContext = null;
                }
                adornerLayer.Remove(adorner);
            }
            adorner = null;
        }
    }
}
