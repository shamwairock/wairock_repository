using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class FixRangeYAxis : YAxis
    {
        public FixRangeYAxis(Scale scale, IAxisDataType dataConverter)
            :base(dataConverter)
        {
            this.AutoScale = false;
            this.Scale = scale;
            this.UpdateScale();
        }

        #region MinValue
        public object MinValue
        {
            get { return (object)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(object), typeof(FixRangeYAxis), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        #region MaxValue
        public object MaxValue
        {
            get { return (object)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(object), typeof(FixRangeYAxis), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion

        protected override void CalculateFixedRange(IList<DataSeries> dataSeries)
        {
            this.calcRangeStartValue = this.MinValue;
            this.calcRangeStopValue = this.MaxValue;
        }

        public override void ReleaseAutoScale()
        {
            this.AutoScale = false;
            UpdateScale();
        }
    }
}
