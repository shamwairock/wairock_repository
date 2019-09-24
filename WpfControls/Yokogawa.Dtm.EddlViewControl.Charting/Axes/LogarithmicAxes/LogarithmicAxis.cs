using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class LogarithmicAxis : AdaptiveRangeAxis, IGridLineSetter, IGridLineProvider
    {
        public LogarithmicAxis(IAxisDataType dataConverter, bool exponentTickLabel=true)
           : base(dataConverter)
        {
            logScale = new LogarithmicScale();
            linearScale = new LinearScale();
            ShowExponentTickLabel = exponentTickLabel;
        }

        private bool showExponentTickLabel = true;
        public bool ShowExponentTickLabel
        {
            get { return showExponentTickLabel; }
            set 
            {
                showExponentTickLabel = value;
                RaiseNotifyPropertyChanged("ShowExponentTickLabel");
            }
        }

        private LogarithmicScale logScale = null;
        public LogarithmicScale LogScale { get { return logScale; } }

        private LinearScale linearScale = null;
        public LinearScale LinearScale { get { return linearScale; } }

        private bool showLogAxis = true;
        internal bool ShowLogAxis { get { return showLogAxis; } set { showLogAxis = value; } }

        protected override void SetScale(object startValue, object stopValue, bool autoRange)
        {
            if (double.IsNaN(TopLeftPosPixels) || double.IsNaN(BottomRightPosPixels) || TopLeftPosPixels == BottomRightPosPixels || DataConverter == null)
            {
                return;
            }
            SetLogarithmicScale(startValue, stopValue, autoRange);
            SetLinearScale(startValue, stopValue, autoRange);
        }

        private void SetLogarithmicScale(object startValue, object stopValue, bool autoRange)
        {
            double ss = logScale.MinDefaultRange;
            double ee = logScale.MaxDefaultRange;

            if (startValue != null && stopValue != null)
            {
                ss = DataConverter.ToAxisDouble(startValue);
                ee = DataConverter.ToAxisDouble(stopValue);
                logScale.GetValidRangeData(ref ss, ref ee, autoRange);
            }
            //if (Math.Abs(ss - ee) < 0.001)
            //{
            //    ss = ss - 0.001;
            //    ee = ee + 0.001;
            //}
            logScale.SetScale(ss, ee, GetStartValuePixelsPos(), GetEndValuePixelsPos(), autoRange);
        }

        private void SetLinearScale(object startValue, object stopValue, bool autoRange)
        {
            double ss = linearScale.MinDefaultRange;
            double ee = linearScale.MaxDefaultRange;

            if (startValue != null && stopValue != null)
            {
                ss = DataConverter.ToAxisDouble(startValue);
                ee = DataConverter.ToAxisDouble(stopValue);
                linearScale.GetValidRangeData(ref ss, ref ee, autoRange);
            }
            linearScale.SetScale(ss, ee, GetStartValuePixelsPos(), GetEndValuePixelsPos(), autoRange);
        }

        public override double ToPixels(object value)
        {
            if (ShowLogAxis)
            {
                if (logScale == null || (!logScale.IsConsistent) || DataConverter == null)
                {
                    throw new InvalidOperationException("scale isn't initialized");
                }
                return logScale.GetPositionByValue(this.DataConverter.ToAxisDouble(value));
            }
            else
            {
                if (linearScale == null || (!linearScale.IsConsistent) || DataConverter == null)
                {
                    throw new InvalidOperationException("scale isn't initialized");
                }
                return linearScale.GetPositionByValue(this.DataConverter.ToAxisDouble(value));
            }
        }

        public override object FromPixels(double value)
        {
            if (ShowLogAxis)
            {
                if (logScale == null || (!logScale.IsConsistent) || DataConverter == null)
                {
                    throw new InvalidOperationException("Scale isn't initialized");
                }
                return this.DataConverter.ToDataValue((double)logScale.GetValueByPoistion((value)));
            }
            else
            {
                if (linearScale == null || (!linearScale.IsConsistent) || DataConverter == null)
                {
                    throw new InvalidOperationException("Scale isn't initialized");
                }
                return this.DataConverter.ToDataValue((double)linearScale.GetValueByPoistion((value)));

            }
        }

        public override double StartPixelsPos
        {
            get
            {
                if (ShowLogAxis)
                {
                    return logScale.GetPositionByValue(logScale.Minimum);
                }
                else
                {
                    return linearScale.GetPositionByValue(linearScale.Minimum);
                }
            }
        }

        public override double StopPixelsPos
        {
            get
            {
                if (ShowLogAxis)
                {
                    return logScale.GetPositionByValue(logScale.Maximum);
                }
                else
                {
                    return linearScale.GetPositionByValue(linearScale.Maximum);
                }
            }
        }

        #region MinValue
        public object MinValue
        {
            get { return (object)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(object), typeof(LogarithmicAxis), 
                new PropertyMetadata(null));
        #endregion

        #region MaxValue
        public object MaxValue
        {
            get { return (object)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(object), typeof(LogarithmicAxis), 
                new PropertyMetadata(null));
        #endregion

        protected override bool HasValidRange()
        {
            return MinValue != null && MaxValue != null && DataConverter.Compare(MinValue, MaxValue) < 0;
        }

        protected override void CalculateFixedRange(IList<DataSeries> dataSeries)
        {
            this.calcRangeStartValue = MinValue;
            this.calcRangeStopValue = MaxValue;
        }

        private GridLinePosition[] gridlines = null;
        public void SetGridLines(GridLinePosition[] lines)
        {
            gridlines = lines;
        }

        public GridLinePosition[] GetGridLines()
        {
            return gridlines;
        }
    }
}
