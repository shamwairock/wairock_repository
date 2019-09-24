using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class SweepLinearXAxis : LinearRealTimeXAxis
    {
        public override object AxisTypeName
        {
            get
            {
                return this.GetType();
            }
        }

        private double currentScaleValue = 0.0;
        private double preStartValue = 0.0;
        private double preEndValue = 0.0;
        private double postStartValue = 0.0;
        private double postEndValue = 0.0;

        private LinearScale prePartScale = null;
        public LinearScale PrePartScale { get { return prePartScale; } }

        private LinearScale postPartScale = null;
        public LinearScale PostPartScale { get { return postPartScale; } }

        private bool postPartValid = false;
        public bool PostPartValid
        {
            get { return postPartValid; }
        }

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

        public SweepLinearXAxis(IAxisDataType dataConverter)
            : base(dataConverter)
        {
            this.prePartScale = new LinearScale();
            this.postPartScale = new LinearScale();
            this.UpdateScale();
        }

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

            preEndValue = currentScaleValue;
            postStartValue = preEndValue - length;
            if (!isFixRange)
            {
                postEndValue = origin + length * num;
                preStartValue = origin + length * num;
                postPartValid = postStartValue >= origin;
            }
            else
            {
                preStartValue = ToAxisDouble(fixedRangeStartValue);
                postEndValue = ToAxisDouble(fixedRangeStopValue);
                if (InRange(origin + length * num, postEndValue, preStartValue))
                {
                    postPartValid = true;
                    rangLength = postEndValue - postStartValue + preEndValue - preStartValue;
                }
                else
                {
                    postPartValid = false;
                    preEndValue = postEndValue;
                    rangLength = preEndValue - preStartValue;
                }
            }
            this.prePartScale.SetScale(preStartValue, preStartValue + rangLength, this.TopLeftPosPixels, this.BottomRightPosPixels, false);
            this.postPartScale.SetScale(postEndValue - rangLength, postEndValue, this.TopLeftPosPixels, this.BottomRightPosPixels, false);
        }

        private IEnumerable<Plot> ToSweepPlot(IEnumerable<IDataPoint> src)
        {
            var prePartPoints = new List<IDataPoint>();
            var postPartPoints = new List<IDataPoint>();
            var latestPoints = new List<IDataPoint>();

            bool prefound = false;
            int preLastPointIndex = -1;

            bool postfound = false;
            int postLastPointIndex = -1;

            int count = src.Count();
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
                    if (!prefound)
                    {
                        prefound = true;
                        if (i > 1)
                        {
                            prePartPoints.Add(src.ElementAt(i - 1));
                        }
                    }
                    prePartPoints.Add(pt);
                    preLastPointIndex = i;
                }

                if (count > 1 && InPostPartRange(x))
                {
                    if (!postfound)
                    {
                        postfound = true;
                        if (i > 1)
                        {
                            postPartPoints.Add(src.ElementAt(i - 1));
                        }
                    }
                    postPartPoints.Add(pt);
                    postLastPointIndex = i;
                }
            }
            if (preLastPointIndex != -1 && preLastPointIndex + 1 >= 0 && preLastPointIndex + 1 < count)
            {
                prePartPoints.Add(src.ElementAt(preLastPointIndex + 1));
            }
            if (postLastPointIndex != -1 && postLastPointIndex + 1 >= 0 && postLastPointIndex + 1 < count)
            {
                postPartPoints.Add(src.ElementAt(postLastPointIndex + 1));
            }

            var ret = new List<Plot>();
            ret.Add(new Plot()
            {
                Points = prePartPoints,
                XScale = prePartScale,
            });
            if (postLastPointIndex != -1 && preLastPointIndex != -1)
            {
                ret.Add(new Plot()
                {
                    Points = postPartPoints,
                    XScale = postPartScale,
                    ClipSettings = new ClipSetting()
                    {
                        XClipRange = new ClipRangeSetting()
                        {
                            StartPoint = src.ElementAt(preLastPointIndex),
                            Scale = prePartScale,
                        }
                    }
                });
            }
            return ret;
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            return ToSweepPlot(src);
        }

        public override double ToPixels(object value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
               this.postPartScale == null || !this.postPartScale.IsConsistent ||
               this.DataConverter == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            var scaleValue = this.DataConverter.ToAxisDouble(value);
            if (!postPartValid || InPrePartRange(scaleValue))
            {
                return this.prePartScale.GetPositionByValue(scaleValue);
            }
            else
            {
                return this.postPartScale.GetPositionByValue(scaleValue);
            }
        }

        public override double ToPixels(object value, Scale scale)
        {
            if (scale == prePartScale || scale == postPartScale)
            {
                var scaleValue = this.DataConverter.ToAxisDouble(value);
                return scale.GetPositionByValue(scaleValue);
            }
            else
            {
                return ToPixels(value);
            }
        }

        public bool InPrePartRange(double value)
        {
            return value >= preStartValue && value <= preEndValue;
        }

        public bool InPostPartRange(double value)
        {
            return value >= postStartValue && value <= postEndValue;
        }

        public override object FromPixels(double value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
                this.postPartScale == null || !this.postPartScale.IsConsistent ||
                this.DataConverter == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }

            var scaleValue = (double)this.prePartScale.GetValueByPoistion(value);
            if (!InPrePartRange(scaleValue))
            {
                scaleValue = (double)this.postPartScale.GetValueByPoistion(value);
            }
            return this.DataConverter.ToDataValue(scaleValue);
        }

        public override AxisOrientation Orientation
        {
            get { return AxisOrientation.Horizontal; }
        }
    }
}