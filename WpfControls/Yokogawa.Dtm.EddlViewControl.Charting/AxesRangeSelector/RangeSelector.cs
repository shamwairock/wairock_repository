using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    [TemplatePart(Name = PARTID_StartThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PARTID_EndThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PARTID_Track, Type = typeof(Canvas))]
    public class RangeSelector : Control
    {
        private Thumb _startThumb;
        private Thumb _endThumb;
        private Canvas _track;

        private const string PARTID_StartThumb = "PART_Start";
        private const string PARTID_EndThumb = "PART_End";
        private const string PARTID_Track = "PART_Track";

        #region Dependency Properties
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnMinimumChanged, CoerceMinimum));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnMaximumChanged, CoerceMaximum));

        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register(
            "RangeStart", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnRangeStartChanged, CoerceRangeStart));

        public static readonly DependencyProperty RangeEndProperty = DependencyProperty.Register(
            "RangeEnd", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, OnRangeEndChanged, CoerceRangeEnd));

        public static readonly DependencyProperty ComputedStartOffsetProperty = DependencyProperty.Register(
            "ComputedStartOffset", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ComputedEndOffsetProperty = DependencyProperty.Register(
            "ComputedEndOffset", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ComputedRangeWidthProperty = DependencyProperty.Register(
            "ComputedRangeWidth", typeof(double), typeof(RangeSelector));

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

        public double ComputedStartOffset
        {
            get { return (double)GetValue(ComputedStartOffsetProperty); }
            private set { SetValue(ComputedStartOffsetProperty, value); }
        }

        public double ComputedEndOffset
        {
            get { return (double)GetValue(ComputedEndOffsetProperty); }
            private set { SetValue(ComputedEndOffsetProperty, value); }
        }

        public double ComputedRangeWidth
        {
            get { return (double)GetValue(ComputedRangeWidthProperty); }
            set { SetValue(ComputedRangeWidthProperty, value); }
        }

        #endregion

        #region Events
        public static readonly RoutedEvent RangeChangedEvent = EventManager.RegisterRoutedEvent("RangeChanged", RoutingStrategy.Bubble, typeof(RangeChangedEventHandler),
                                             typeof(RangeSelector));

        public event RangeChangedEventHandler RangeChanged
        {
            add { AddHandler(RangeChangedEvent, value); }
            remove { RemoveHandler(RangeChangedEvent, value); }
        }

        #endregion

        static RangeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSelector), new FrameworkPropertyMetadata(typeof(RangeSelector)));
            FocusableProperty.OverrideMetadata(typeof(RangeSelector), new FrameworkPropertyMetadata(true));
        }

        public RangeSelector()
        {
            Loaded += delegate
            {
                RefreshDisplay();
            };

            SizeChanged += delegate
            {
                RefreshDisplay();
            };
        }

        private void RefreshDisplay()
        {
            if (_startThumb != null)
            {
                ComputedStartOffset = CalcY(RangeStart, _startThumb, true);
            }
            if (_endThumb != null)
            {
                ComputedEndOffset = CalcY(RangeEnd, _endThumb, false);
            }
            ComputedRangeWidth = ComputedEndOffset - ComputedStartOffset;
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(RangeStartProperty);
            d.CoerceValue(RangeEndProperty);
            d.CoerceValue(MaximumProperty);

            RangeSelector selector = (RangeSelector)d;
            if (!selector.IsLoaded) return;
            selector.ComputedStartOffset = selector.CalcY(selector.RangeStart, selector._startThumb, true);
            selector.ComputedEndOffset = selector.CalcY(selector.RangeEnd, selector._endThumb, false);
            selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(RangeStartProperty);
            d.CoerceValue(RangeEndProperty);

            RangeSelector selector = (RangeSelector)d;
            if (!selector.IsLoaded) return;
            selector.ComputedStartOffset = selector.CalcY(selector.RangeStart, selector._startThumb, true);
            selector.ComputedEndOffset = selector.CalcY(selector.RangeEnd, selector._endThumb, false);
            selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;
        }

        private static object CoerceMinimum(DependencyObject d, object basevalue)
        {
            double newValue = (double)basevalue;
            RangeSelector selector = (RangeSelector)d;
            if (newValue > selector.RangeStart) return selector.RangeStart;

            return basevalue;
        }

        private static object CoerceMaximum(DependencyObject d, object basevalue)
        {
            double newValue = (double)basevalue;
            RangeSelector selector = (RangeSelector)d;
            if (newValue < selector.RangeEnd) return selector.RangeEnd;

            return basevalue;
        }

        private static void OnRangeStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RangeSelector selector = (RangeSelector)d;
            d.CoerceValue(MinimumProperty);
            d.CoerceValue(RangeEndProperty);

            if (!selector.IsLoaded) return;
            selector.ComputedStartOffset = selector.CalcY((double)e.NewValue, selector._startThumb, true);
            selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;

            selector.RaiseRangeChangedEvent((double)e.NewValue, selector.RangeEnd);
        }

        private static void OnRangeEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RangeSelector selector = (RangeSelector)d;
            d.CoerceValue(RangeStartProperty);
            d.CoerceValue(MaximumProperty);

            if (!selector.IsLoaded) return;
            selector.ComputedEndOffset = selector.CalcY((double)e.NewValue, selector._endThumb, false);
            selector.ComputedRangeWidth = selector.ComputedEndOffset - selector.ComputedStartOffset;

            selector.RaiseRangeChangedEvent(selector.RangeStart, (double)e.NewValue);
        }

        private void RaiseRangeChangedEvent(double start, double end)
        {
            if (AutomationPeer.ListenerExists(AutomationEvents.PropertyChanged))
            {
                RangeSelectorAutomationPeer peer = (RangeSelectorAutomationPeer)UIElementAutomationPeer.FromElement(this);
                if (peer != null)
                {
                    peer.RaiseRangeChangedEvent(start, end);
                }
            }
            
            var args = new RangeChangedEventArgs(start, end);
            args.Source = this;
            RaiseEvent(args);
        }

        private static object CoerceRangeStart(DependencyObject d, object basevalue)
        {
            RangeSelector selector = (RangeSelector)d;
            double newStart = (double)basevalue;
            if (newStart < selector.Minimum) return selector.Minimum;
            if (newStart > selector.RangeEnd) return selector.RangeEnd;

            return basevalue;
        }

        private static object CoerceRangeEnd(DependencyObject d, object basevalue)
        {
            RangeSelector selector = (RangeSelector)d;
            double newValue = (double)basevalue;
            if (newValue < selector.RangeStart) return selector.RangeStart;
            if (newValue > selector.Maximum) return selector.Maximum;

            return basevalue;
        }

        private double CalcY(double value, Thumb thumb, bool startThumb)
        {
            double denom = Maximum - Minimum;
            if (denom <= 0) denom = 1;

            double newValue = ((value - Minimum) / denom) * (_track.ActualHeight - thumb.ActualHeight * 2);
            newValue += startThumb ? 0 : thumb.ActualHeight;
            return Math.Min(newValue, _track.ActualHeight - thumb.ActualHeight);
        }

        public override void OnApplyTemplate()
        {
            // Track
            _track = GetTemplateChild(PARTID_Track) as Canvas;
            if (_track == null) return;

            _track.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
            {
                RangeStart = Minimum;
                RangeEnd = Maximum;
            };


            // Start Thumb
            _startThumb = GetTemplateChild(PARTID_StartThumb) as Thumb;
            if (_startThumb != null)
            {
                SetupThumb(_startThumb, () => ComputedStartOffset, (y) =>
                {
                    if (y < RangeEnd)
                    {
                        RangeStart = y;
                    }
                });
            }

            // End Thumb
            _endThumb = GetTemplateChild(PARTID_EndThumb) as Thumb;
            if (_endThumb != null)
            {
                SetupThumb(_endThumb, () => ComputedEndOffset, (y) =>
                {
                    if(y > RangeStart)
                    {
                        RangeEnd = y;
                    }
                });
            }
        }

        private void SetupThumb(Thumb thumb, Func<double> getOffset, Action<double> updaterAction)
        {
            thumb.DragDelta += delegate (object sender, DragDeltaEventArgs args)
            {
                double change = getOffset() + args.VerticalChange;
                if (change < 0)
                    change = 0;
                else if (change > _track.ActualHeight - thumb.Height * 2)
                    change = _track.ActualHeight - thumb.ActualHeight * 2;

                double ratio = change / (_track.ActualHeight - thumb.ActualHeight * 2);
                double newValue = Minimum + ratio * (Maximum - Minimum);

                updaterAction(newValue);
            };
            thumb.DragCompleted += delegate (object sender, DragCompletedEventArgs args)
            {
                //double change = getOffset() + args.VerticalChange;
                //if (change < 0)
                //    change = 0;
                //else if (change > _track.ActualHeight - thumb.Height * 2)
                //    change = _track.ActualHeight - thumb.ActualHeight * 2;

                //double ratio = change / (_track.ActualHeight - thumb.ActualHeight * 2);
                //double newValue = Minimum + ratio * (Maximum - Minimum);

                //updaterAction(newValue);
            };
        }

        #region Automation
        
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RangeSelectorAutomationPeer(this);
        }
        
        #endregion
    }

    public class RangeSelectorAutomationPeer : FrameworkElementAutomationPeer, IValueProvider
    {
        private string _currentValue;

        public RangeSelectorAutomationPeer(RangeSelector owner)
            : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        protected override string GetClassNameCore()
        {
            return OwningRangeSelector.GetType().Name;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Value:
                    return this;
            }

            return base.GetPattern(patternInterface);
        }

        public void SetValue(string value)
        {
            string[] range = value.Split(new[] { ',' });
            if (range.Length == 2)
            {
                double start, end;

                // Set the range only if both values parse correctly
                if (double.TryParse(range[0], out start) &&
                    double.TryParse(range[1], out end))
                {
                    OwningRangeSelector.RangeStart = start;
                    OwningRangeSelector.RangeEnd = end;
                }
            }
        }

        public string Value
        {
            get
            {
                return FormatValue(OwningRangeSelector.RangeStart, OwningRangeSelector.RangeEnd);
            }
        }

        private static string FormatValue(double start, double end)
        {
            return string.Format("{0:F2},{1:F2}", start, end);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        private RangeSelector OwningRangeSelector
        {
            get { return (RangeSelector)Owner; }
        }

        public void RaiseRangeChangedEvent(double start, double end)
        {
            string newValue = FormatValue(start, end);
            RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, _currentValue, newValue);

            _currentValue = newValue;
        }

    }

    public class RangeChangedEventArgs : RoutedEventArgs
    {
        public double RangeStart { get; private set; }
        public double RangeEnd { get; private set; }

        public RangeChangedEventArgs(double start, double end)
        {
            RangeStart = start;
            RangeEnd = end;
            RoutedEvent = RangeSelector.RangeChangedEvent;
        }
    }

    public delegate void RangeChangedEventHandler(object sender, RangeChangedEventArgs args);
}
