using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ScopeLinearXAxis : LinearRealTimeXAxis
    {
        public override object AxisTypeName
        {
            get
            {
                return this.GetType();
            }
        }

        protected LinearScale prePartScale = null;

        public LinearScale PrePartScale { get { return prePartScale; } }

        public override double StartPixelsPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.GetPositionByValue(this.prePartScale.Minimum);
                }
                return 0;
            }
        }

        public override double StopPixelsPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.GetPositionByValue(this.prePartScale.Maximum);
                }
                return 0;
            }
        }

        public ScopeLinearXAxis(IAxisDataType dataConverter)
            : base(dataConverter)
        {
            this.prePartScale = new LinearScale();
            this.UpdateScale();
        }

        protected double currentScaleValue = 0.0;
        protected double preStartValue = 0.0;
        protected double preEndValue = 0.0;

        public override void CalculateRange(IList<DataSeries> dataSeries)
        {
            if (this.DataConverter == null || dataSeries == null)
            {
                return;
            }

            foreach (var series in dataSeries)
            {
                if (series.Points == null || series.Points.Count == 0)
                {
                    continue;
                }

                Range<IDataPoint> range;
                if (series.Points.GetRangeX(out range))
                {
                    var pt = range.Max;
                    if (currentScaleValue < this.DataConverter.ToAxisDouble(pt.X))
                    {
                        currentScaleValue = this.DataConverter.ToAxisDouble(pt.X);
                    }
                }
            }
            UpdateScale();
        }

        public override void UpdateScale()
        {
            if (this.DataConverter == null)
            {
                return;
            }

            double origin = 0.0;
            double length = this.DataConverter.DataLengthToDoubleAxisLength(this.DisplayLength);
            double rangLength = length;
            double s = origin;
            double e = origin + length;
            double diff = currentScaleValue - origin;
            double count = diff / length;

            long num = (count > (long)count) ? ((long)count) : ((long)count - 1);
            num = num < 0 ? 0 : num;

            if (!isFixRange)
            {
                preStartValue = origin + length * num;
                preEndValue = origin + length * (num + 1);
            }
            else
            {
                preStartValue = ToAxisDouble(fixedRangeStartValue);
                preEndValue = ToAxisDouble(fixedRangeStopValue);
                rangLength = preEndValue - preStartValue;
            }
            this.prePartScale.SetScale(preStartValue, preStartValue + rangLength, this.TopLeftPosPixels, this.BottomRightPosPixels, false);
        }

            private IEnumerable<Plot> ToStripPlot(IEnumerable<IDataPoint> src)
        {
            var prePartPoints = new List<IDataPoint>();
            var postPartPoints = new List<IDataPoint>();

            int count = src.Count();
            bool foundPoint = false;
            int lastPointIndex = -1;
            for (int i = 0; i < count; i++)
            {
                var pt = src.ElementAt(i);
                double x;
                try
                {
                    x = ToAxisDouble(pt.X);
                }
                catch (FormatException)
                {
                    continue;
                }
                if (InPrePartRange(x))
                {
                    if (!foundPoint)
                    {
                        foundPoint = true;
                        if (i > 1)
                        {
                            prePartPoints.Add(src.ElementAt(i - 1));
                        }
                    }
                    prePartPoints.Add(pt);
                    lastPointIndex = i;
                }
            }
            if (lastPointIndex != -1 && lastPointIndex + 1 >= 0 && lastPointIndex + 1 < count)
            {
                prePartPoints.Add(src.ElementAt(lastPointIndex + 1));
            }
            var ret = new List<Plot>();
            ret.Add(new Plot() { Points = prePartPoints });
            return ret;
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            return ToStripPlot(src);
        }

        public override double ToPixels(object value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
                this.DataConverter == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            var scaleValue = this.DataConverter.ToAxisDouble(value);
            return this.prePartScale.GetPositionByValue(scaleValue);
        }

        protected bool InPrePartRange(double value)
        {
            return value >= preStartValue && value <= preEndValue;
        }

        public override object FromPixels(double value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
                this.DataConverter == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            var scaleValue = (double)this.prePartScale.GetValueByPoistion(value);
            return this.DataConverter.ToDataValue(scaleValue);
        }

        //protected RealTimeStripTicksCreater ticksCreater = new RealTimeStripTicksCreater();

        public override AxisOrientation Orientation
        {
            get { return AxisOrientation.Horizontal; }
        }
    }
}
