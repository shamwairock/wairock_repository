using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class LinearAxis : AdaptiveRangeAxis, IGridLineSetter, IGridLineProvider
    {
        public LinearAxis(IAxisDataType dataConverter)
           : base(dataConverter)
        {
            scale = new LinearScale();
        }

        private LinearScale scale = null;
        public LinearScale Scale { get { return scale; } }

        protected override void SetScale(object startValue, object stopValue, bool autoRange)
        {
            if (double.IsNaN(TopLeftPosPixels) || double.IsNaN(BottomRightPosPixels) || TopLeftPosPixels == BottomRightPosPixels || DataConverter == null)
            {
                return;
            }
            double ss = scale.MinDefaultRange;
            double ee = scale.MaxDefaultRange;

            if (startValue != null && stopValue != null)
            {
                ss = DataConverter.ToAxisDouble(startValue);
                ee = DataConverter.ToAxisDouble(stopValue);
                scale.GetValidRangeData(ref ss, ref ee, autoRange);
            }
            scale.SetScale(ss, ee, GetStartValuePixelsPos(), GetEndValuePixelsPos(), autoRange);
        }

        public override double ToPixels(object value)
        {
            if (scale == null || (!scale.IsConsistent) || DataConverter == null)
            {
                throw new InvalidOperationException("scale isn't initialized");
            }
            return scale.GetPositionByValue(this.DataConverter.ToAxisDouble(value));
        }

        public override object FromPixels(double value)
        {
            if (scale == null || (!scale.IsConsistent) || DataConverter == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            return this.DataConverter.ToDataValue((double)scale.GetValueByPoistion((value)));
        }

        public override double StartPixelsPos
        {
            get { return scale.GetPositionByValue(scale.Minimum); }
        }

        public override double StopPixelsPos
        {
            get { return scale.GetPositionByValue(scale.Maximum); }
        }

        #region MinValue
        public object MinValue
        {
            get { return (object)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(object), typeof(LinearAxis), 
                new FrameworkPropertyMetadata(null));
        #endregion

        #region MaxValue
        public object MaxValue
        {
            get { return (object)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(object), typeof(LinearAxis), 
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
