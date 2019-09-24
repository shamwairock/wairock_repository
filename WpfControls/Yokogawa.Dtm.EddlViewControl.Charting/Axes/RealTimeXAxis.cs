using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class RealTimeXAxis : XAxis
    {
        private double? minMayorTickInterval = null;
        public virtual double? MinMayorTickInterval
        {
            get { return minMayorTickInterval; }
            set { minMayorTickInterval = value; }
        }

        private double? maxMayorTickInterval = null;
        public virtual double? MaxMayorTickInterval
        {
            get { return maxMayorTickInterval; }
            set { maxMayorTickInterval = value; }
        }

        public RealTimeXAxis(IAxisDataType dataConverter)
            :base(dataConverter)
        {
        }

        #region DisplayLength
        private object displayLength = 100.0;
        public object DisplayLength
        {
            get { return displayLength; }
            set { displayLength = value; UpdateScale(); }
        }
        #endregion DisplayLength

        protected object ToDataValue(double value)
        {
            return this.AxisDataManager.ToDataValue(value);
        }

        protected double ToAxisDouble(object value)
        {
            return this.AxisDataManager.ToAxisDouble(value);
        }

        protected TickInfo[] mayorTicks = null;
        protected TickInfo[] minorTicks = null;
        public override TickInfo[] MayorTicks
        {
            get { return mayorTicks; }
        }

        protected bool InRange(double value, double rangeLeft, double rangeRight)
        {
            return value >= rangeLeft && value <= rangeRight;
        }

        #region Fixed to range
        //protected bool isFixRange = false;
        protected object rangeStart = null;
        protected object rangeEnd = null;
        public override void FixRangeTo(object start, object end)
        {
            isFixRange = true;
            rangeStart = start;
            rangeEnd = end;
            UpdateScale();
        }
        public override void ReleaseFixRange()
        {
            isFixRange = false;
            UpdateScale();
        }
        #endregion

        public override void ReleaseAutoScale()
        {
            this.AutoScale = false;
            UpdateScale();
        }
    }

    public class RealTimeSweepXAxis : RealTimeXAxis
    {
        private double currentScaleValue = 0.0;
        private double preStartValue = 0.0;
        private double preEndValue = 0.0;
        private double postStartValue = 0.0;
        private double postEndValue = 0.0;

        private Scale prePartScale = null;
        private Scale postPartScale = null;

        private bool postPartValid = false;

        public override double StartPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.ToPixels(this.prePartScale.Start);
                }
                    return 0;
            }
        }

        public override double StopPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.ToPixels(this.prePartScale.Stop);
                }
                    return 0;
            }
        }

        public override bool IsConsistent
        {
            get
            {
                var ret = false;
                if (this.prePartScale != null)
                {
                    ret = this.prePartScale.IsConsistent;
                }
                if (ret && postPartValid)
                {
                    ret = false;
                    if (this.postPartScale != null)
                    {
                        ret = this.postPartScale.IsConsistent;
                    }
                }
                return ret;
            }
        }

        public override double? MinMayorTickInterval
        {
            get
            {
                return base.MinMayorTickInterval;
            }
            set
            {
                base.MinMayorTickInterval = value;
                (this.prePartScale as LinearScale).MinMayorTickInterval = value;
                (this.postPartScale as LinearScale).MinMayorTickInterval = value;
            }
        }

        public override double? MaxMayorTickInterval
        {
            get
            {
                return base.MaxMayorTickInterval;
            }
            set
            {
                base.MaxMayorTickInterval = value;
                (this.prePartScale as LinearScale).MaxMayorTickInterval = value;
                (this.postPartScale as LinearScale).MaxMayorTickInterval = value;
            }
        }

        public RealTimeSweepXAxis(IAxisDataType dataConverter)
            :base(dataConverter)
        {
            this.prePartScale = new LinearScale();
            this.postPartScale = new LinearScale();
            this.UpdateScale();
        }

        public override void CalculateRange(IList<DataSeries> dataSeries)
        {
            if (this.AxisDataManager == null || dataSeries == null)
            {
                return;
            }
            foreach (var series in dataSeries)
            {
                if (series.Points == null || series.Points.Count == 0)
                {
                    continue;
                }
                var pt = series.Points.Last();
                if (currentScaleValue < this.AxisDataManager.ToAxisDouble(pt.X))
                {
                    currentScaleValue = this.AxisDataManager.ToAxisDouble(pt.X);
                }
            }
            UpdateScale();
        }

        public override void UpdateScale()
        {
            if (this.AxisDataManager == null)
            {
                return;
            }

            double origin = 0.0;
            double length = this.AxisDataManager.DataLengthToDoubleAxisLength(this.DisplayLength);
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
                preStartValue = ToAxisDouble(rangeStart);
                postEndValue = ToAxisDouble(rangeEnd);
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
            foreach (var pt in src)
            {
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
                    prePartPoints.Add(pt);
                }
                if (InPostPartRange(x))
                {
                    postPartPoints.Add(pt);
                }
            }
            var ret = new List<Plot>();
            ret.Add(new Plot() { Points = prePartPoints });
            ret.Add(new Plot() { Points = postPartPoints });
            return ret;
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            return ToSweepPlot(src);
        }

        private int mayorTicksCount = 0;
        public override TickInfo[] GetTicks(int tickCount)
        {
            var retTicks = new List<TickInfo>();

            var preTicks = this.prePartScale.GetTicks(tickCount);
            var postTicks = this.postPartScale.GetTicks(tickCount);


            if (preTicks != null && preTicks.Count > 0)
            {
                var preRetTicks = (from tick in preTicks
                                   where (!postPartValid) || InPrePartRange((double)tick.Value)
                                   select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong))
                               .ToArray();

                retTicks.AddRange(preRetTicks);
                mayorTicksCount = tickCount;
            }
            if(postTicks != null && postTicks.Count > 0)
            {
                var postRetTicks = (from tick in postTicks
                                    where InPostPartRange((double)tick.Value)
                                    select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong))
                                    .ToArray();

                if (postPartValid)
                {
                    retTicks.AddRange(postRetTicks);
                }
                mayorTicksCount = tickCount;
            }
            mayorTicks = retTicks.ToArray();
            return mayorTicks;
            //return retTicks.ToArray();
        }

        public override TickInfo[] GetMinorTicks(int ticksCount)
        {
            var retTicks = new List<TickInfo>();
            var preTicks = this.prePartScale.GetMinorTicks(mayorTicksCount, ticksCount);
            var postTicks = this.postPartScale.GetMinorTicks(mayorTicksCount, ticksCount);

            if (preTicks != null && preTicks.Count > 0)
            {
                var preRetTicks = (from tick in preTicks
                                   where (!postPartValid) || InPrePartRange((double)tick.Value)
                                   select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong))
                               .ToArray();
                retTicks.AddRange(preRetTicks);
            }

            if (postTicks != null && postTicks.Count > 0)
            {
                var postRetTicks = (from tick in postTicks
                                    where InPostPartRange((double)tick.Value)
                                    select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong))
                                    .ToArray();
                if (postPartValid)
                {
                    retTicks.AddRange(postRetTicks);
                }
            } 
         
            minorTicks = retTicks.ToArray();
            return minorTicks;
            //return retTicks.ToArray();
        }

        public override double ToPixels(object value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
               this.postPartScale == null || !this.postPartScale.IsConsistent ||
               this.AxisDataManager == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }

            var scaleValue = this.AxisDataManager.ToAxisDouble(value);

            if (!postPartValid || InPrePartRange(scaleValue))
            {
                return this.prePartScale.ToPixels(scaleValue);
            }
            else
            {
                return this.postPartScale.ToPixels(scaleValue);
            }
        }

        private bool InPrePartRange(double value)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("pre: {0}-{1}:{2}", preStartValue, preEndValue, pixels));
            //System.Diagnostics.Debug.Assert(preEndValue >= preStartValue);
            return InRange(value, preStartValue , preEndValue);
        }
        private bool InPostPartRange(double value)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("post: {0}-{1}:{2}", postStartValue, postEndValue, pixels));
            //System.Diagnostics.Debug.Assert(postEndValue >= postStartValue);
            return InRange(value, postStartValue, postEndValue);
        }

        public override object FromPixels(double value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent  ||
                this.postPartScale == null || !this.postPartScale.IsConsistent ||
                this.AxisDataManager == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }

            var scaleValue = (double)this.prePartScale.FromPixels(value);
            if (!InPrePartRange(scaleValue))
            {
                scaleValue = (double)this.postPartScale.FromPixels(value);
            }
            return this.AxisDataManager.ToDataValue(scaleValue);
        }
    }

    public class RealTimeStripXAxis : RealTimeXAxis
    {
        protected double currentScaleValue = 0.0;
        protected double preStartValue = 0.0;
        protected double preEndValue = 0.0;

        protected Scale prePartScale = null;

        public override double StartPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.ToPixels(this.prePartScale.Start);
                }
                return 0;
            }
        }

        public override double StopPos
        {
            get
            {
                if (this.prePartScale != null)
                {
                    return this.prePartScale.ToPixels(this.prePartScale.Stop);
                }
                return 0;
            }
        }

        public override bool IsConsistent
        {
            get
            {
                var ret = false;
                if (this.prePartScale != null)
                {
                    ret = this.prePartScale.IsConsistent;
                }
                return ret;
            }
        }

        public override double? MinMayorTickInterval
        {
            get
            {
                return base.MinMayorTickInterval;
            }
            set
            {
                base.MinMayorTickInterval = value;
                (this.prePartScale as LinearScale).MinMayorTickInterval = value;
                //(this.postPartScale as LinearScale).MinMayorTickInterval = pixels;
            }
        }

        public override double? MaxMayorTickInterval
        {
            get
            {
                return base.MaxMayorTickInterval;
            }
            set
            {
                base.MaxMayorTickInterval = value;
                (this.prePartScale as LinearScale).MaxMayorTickInterval = value;
                //(this.postPartScale as LinearScale).MaxMayorTickInterval = pixels;
            }
        }

        public RealTimeStripXAxis(IAxisDataType dataConverter)
            : base(dataConverter)
        {
            this.prePartScale = new LinearScale();
            this.UpdateScale();
        }

        public override void CalculateRange(IList<DataSeries> dataSeries)
        {
            if (this.AxisDataManager == null || dataSeries == null)
            {
                return;
            }

            foreach (var series in dataSeries)
            {
                if (series.Points == null || series.Points.Count == 0)
                {
                    continue;
                }
                var pt = series.Points.Last();
                if (currentScaleValue < this.AxisDataManager.ToAxisDouble(pt.X))
                {
                    currentScaleValue = this.AxisDataManager.ToAxisDouble(pt.X);
                }
            }
            UpdateScale();
        }

        public override void UpdateScale()
        {
            if (this.AxisDataManager == null)
            {
                return;
            }

            double origin = 0.0;
            double length = this.AxisDataManager.DataLengthToDoubleAxisLength(this.DisplayLength);
            double rangLength = length;
            double s = origin;
            double e = origin + length;
            double diff = currentScaleValue - origin;
            double count = diff / length;

            long num = (count > (long)count) ? ((long)count) : ((long)count - 1);
            num = num < 0 ? 0 : num;

            if (!isFixRange)
            {
                preStartValue = currentScaleValue - length;
                preEndValue = currentScaleValue;
                if (preStartValue < origin)
                {
                    preStartValue = origin + length * num;
                    preEndValue = origin + length * (num + 1);
                }
            }
            else
            {
                preStartValue = ToAxisDouble(rangeStart);
                preEndValue = ToAxisDouble(rangeEnd);
                rangLength = preEndValue - preStartValue;
            }
            this.prePartScale.SetScale(preStartValue, preStartValue + rangLength, this.TopLeftPosPixels, this.BottomRightPosPixels, false);
        }

        private IEnumerable<Plot> ToStripPlot(IEnumerable<IDataPoint> src)
        {
            var prePartPoints = new List<IDataPoint>();
            var postPartPoints = new List<IDataPoint>();
            foreach (var pt in src)
            {
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
                    prePartPoints.Add(pt);
                }
            }
            var ret = new List<Plot>();
            ret.Add(new Plot() { Points = prePartPoints });
            return ret;
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            return ToStripPlot(src);
        }


        private int mayorTicksCount = 0;
        public override TickInfo[] GetTicks(int tickCount)
        {
            var retTicks = new List<TickInfo>();

            var preTicks = this.prePartScale.GetTicks(tickCount);

            if (preTicks != null && preTicks.Count > 0)
            {
                var preRetTicks = (from tick in preTicks
                                   where InPrePartRange((double)tick.Value)
                                   select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong)).ToArray();
                mayorTicksCount = tickCount;
                retTicks.AddRange(preRetTicks);
            }
            mayorTicks = retTicks.ToArray();
            return mayorTicks;
            //return retTicks.ToArray(); 
        }

        public override TickInfo[] GetMinorTicks(int ticksCount)
        {
            var retTicks = new List<TickInfo>();
            var preTicks = this.prePartScale.GetMinorTicks(mayorTicksCount, ticksCount);

            if (preTicks != null && preTicks.Count > 0)
            {
                var preRetTicks = (from tick in preTicks
                                   where InPrePartRange((double)tick.Value)
                                   select new TickInfo(ToDataValue((double)tick.Value), tick.TickPos, tick.IsLong)).ToArray();
                retTicks.AddRange(preRetTicks);
            }
            minorTicks = retTicks.ToArray();
            return minorTicks;
            //return retTicks.ToArray(); 
        }

        public override double ToPixels(object value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent ||
                this.AxisDataManager == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            var scaleValue = this.AxisDataManager.ToAxisDouble(value);
            return this.prePartScale.ToPixels(scaleValue);
        }

        protected bool InPrePartRange(double value)
        {
            return InRange(value, preStartValue, preEndValue);
        }

        public override object FromPixels(double value)
        {
            if (this.prePartScale == null || !this.prePartScale.IsConsistent || 
                this.AxisDataManager == null)
            {
                throw new InvalidOperationException("Scale isn't initialized");
            }
            var scaleValue = (double)this.prePartScale.FromPixels(value);
            return this.AxisDataManager.ToDataValue(scaleValue);
        }

        protected override void CalculateFixedRange(IList<DataSeries> dataSeries)
        {
            throw new NotImplementedException();
        }
    }

    public class RealTimeScopeXAxis : RealTimeStripXAxis
    {
        public RealTimeScopeXAxis(IAxisDataType dataConverter)
            :base(dataConverter)
        {
        }
        public override void UpdateScale()
        {
            if (this.AxisDataManager == null)
            {
                return;
            }

            double origin = 0.0;
            double length = this.AxisDataManager.DataLengthToDoubleAxisLength(this.DisplayLength);
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
                preStartValue = ToAxisDouble(rangeStart);
                preEndValue = ToAxisDouble(rangeEnd);
                rangLength = preEndValue - preStartValue;
            }
            this.prePartScale.SetScale(preStartValue, preStartValue + rangLength, this.TopLeftPosPixels, this.BottomRightPosPixels, false);
        }
    }
}
