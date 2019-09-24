using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearTicksCreater : ITicksCreater<double>
    {
        private Scale scale = null;
        public Scale Scale
        {
            get { return scale; }
            set { scale = value; minorTickCreater.Scale = scale; }
        }

        private double mayorTickLength = 10;
        public double MayorTickLength
        {
            get { return mayorTickLength; }
            set { mayorTickLength = value; minorTickCreater.SetMayorTickLength(value);  }
        }

        private double mayorRangeStart;
        private double mayorRangeStop;
        public void SetRange(double start, double stop)
        {
            this.mayorRangeStart = start;
            this.mayorRangeStop = stop;
        }

        private double? minMayorTickInterval = null;
        public double? MinMayorTickInterval
        {
            get { return minMayorTickInterval; }
            set { minMayorTickInterval = value; }
        }

        private double? maxMayorTickInterval = null;
        public double? MaxMayorTickInterval
        {
            get { return maxMayorTickInterval; }
            set { maxMayorTickInterval = value; }
        }

        protected virtual double UpdateMayorInterval(double step, double? minInterval, double? maxInterval)
        {
            if (minInterval.HasValue && minInterval.Value.CompareTo(step) > 0)
            {
                return minInterval.Value;
            }
            if (maxInterval.HasValue && maxInterval.Value.CompareTo(step) < 0)
            {
                return maxInterval.Value;
            }
            return step;
        }

        public TickInfo[] GetTicks(double start, double stop, int ticksCount)
        {
            var retTicks = new List<TickInfo>();
            if (start == stop || Scale == null)
            {
                return retTicks.ToArray();
            }

            double ss = start < stop ? start : stop;
            double ee = start > stop ? start : stop;
            double[] ticks = null;

            var wilext = new WilkinsonExtended();
            double outlmin;
            double outlmax;
            double outlstep;

            if (wilext.Easy_wilk_ext(ss, ee, ticksCount, -1, out outlmin, out outlmax, out outlstep))
            {
                ticks = CreateTicks(outlmin, outlmax, outlstep);
            }
            else
            {
                ticks = new double[] { };
            }
            double step = outlstep;
            //double step = CalculateTickStep(ss, ee, ticksCount);
            //step = step = UpdateMayorInterval(step, this.MinMayorTickInterval, this.MaxMayorTickInterval);
            //double[] ticks = null;
            //if (step != 0.0)
            //{
            //    ticks = CreateTicks(ss, ee, step);
            //}
            //else
            //{
            //    ticks = new double[] { };
            //}

            SetMinorTickRanges(ticks, step);

            int index = 0;
            while (index < ticks.Length)
            {
                if (ticks[index] >= start && ticks[index] <= stop)
                {
                    retTicks.Add(new TickInfo(ticks[index], Scale.ToPixels(ticks[index]), true, MayorTickLength));
                }
                index++;
            }
            return retTicks.ToArray();
        }

        private void SetMinorTickRanges(double[] ticks, double step)
        {
            var ranges = new List<Range<double>>();
            if (ticks.Length > 0)
            {
                ranges.Add(new Range<double>(ticks[0] - step, ticks[0]));
                for (int i = 1; i < ticks.Length; i++)
                {
                    ranges.Add(new Range<double>(ticks[i - 1], ticks[i]));
                }
                ranges.Add(new Range<double>(ticks[ticks.Length - 1], ticks[ticks.Length - 1] + 1));
            }
            minorTickCreater.SetRanges(ranges, mayorRangeStart, mayorRangeStop);
        }

        //protected virtual double CalculateTickStep(double ss, double ee, int count)
        //{
        //    double delta = ee - ss;

        //    int log = (int)Math.Round(Math.Log10(delta));

        //    double newStart = RoundHelper.Round(ss, log);

        //    double newStop = RoundHelper.Round(ee, log);

        //    if (newStart == newStop)
        //    {
        //        log--;
        //        newStart = RoundHelper.Round(ss, log);
        //        newStop = RoundHelper.Round(newStop, log);
        //    }

        //    if (newStop < newStart)
        //    {
        //        var t = newStart;
        //        newStart = newStop;
        //        newStop = t;
        //    }

        //    // calculating step between ticks
        //    double unroundedStep = (newStop - newStart) / count;
        //    int stepLog = log;
        //    // trying to round step
        //    double step = RoundHelper.Round(unroundedStep, stepLog);
        //    if (step == 0)
        //    {
        //        stepLog--;
        //        step = RoundHelper.Round(unroundedStep, stepLog);
        //        if (step == 0)
        //        {
        //            // step will not be rounded if attempts to be rounded to zero.
        //            step = unroundedStep;
        //        }
        //    }

        //    return step;
        //}

        protected virtual double[] CreateTicks(double start, double finish, double step)
        {
            double x = step * Math.Floor(start / step);

            if (x == x + step)
            {
                return new double[0];
            }

            List<double> res = new List<double>();

            double increasedFinish = finish + step * 1.05;
            while (x <= increasedFinish)
            {
                res.Add(x);
                x += step;
            }
            return res.ToArray();
        }

        private int[] defaultTickCountArray = new int[] { 20, 10, 5, 4, 3, 2, 1 };
        private int[] tickCountArray = new int[] { 10, 5, 4, 3, 2, 1 };
        public virtual int[] TickCountArray
        {
            get
            {
                if (tickCountArray == null || tickCountArray.Length == 0)
                {
                    return defaultTickCountArray;
                }
                return tickCountArray;
            }
            set { tickCountArray = value; }
        }

        private int defaultTicksCount = 10;
        public virtual int DefaultTicksCount
        {
            get
            {
                if (defaultTicksCount <= 0)
                {
                    return 10;
                }
                return defaultTicksCount;
            }
            set { defaultTicksCount = value; }
        }

        protected virtual int[] GetTickCountsCore()
        {
            return TickCountArray;
        }

        public virtual int DecreaseTickCount(int tickCount)
        {
            return GetTickCountsCore().FirstOrDefault(tick => tick < tickCount);
        }

        public virtual int IncreaseTickCount(int tickCount)
        {
            int newTickCount = GetTickCountsCore().Reverse().FirstOrDefault(tick => tick > tickCount);
            if (newTickCount == 0)
                newTickCount = GetTickCountsCore()[0];
            return newTickCount;
        }

        public int DefaultTickCount()
        {
            return DefaultTicksCount;
        }

        private MinorLinearTicksCreator minorTickCreater = new MinorLinearTicksCreator();
        public ITicksCreater MinorTickCreater
        {
            get { return minorTickCreater; }
        }

        public TickInfo[] GetTicks(int ticksCount)
        {
            return GetTicks(mayorRangeStart, mayorRangeStop, ticksCount);
        }
    }

    public class MinorLinearTicksCreator : ITicksCreater<double>
    {
        private Range<double>[] ranges;
        private double mayorRangeStart;
        private double mayorRangeStop;
        internal void SetRanges(IEnumerable<Range<double>> ranges, double start, double stop)
        {
            this.ranges = ranges.ToArray();
            this.mayorRangeStart = start;
            this.mayorRangeStop = stop;
        }

        private double mayorTickLength = 10;
        internal void SetMayorTickLength(double tickLength)
        {
            mayorTickLength = tickLength;
        }

        public MinorLinearTicksCreator()
        {
            Coeffs = new double[] { 0.6,0.6,0.6,0.6,0.6,0.6,0.6,0.6,0.6 };
        }

        public MinorLinearTicksCreator(double[] coeffs)
            :this()
        {
            if (coeffs != null && coeffs.Length > 0)
            {
                Coeffs = coeffs;
            }
        }

        public Scale Scale { get; set; }
        public double[] Coeffs { get; set; }
        public TickInfo[] GetTicks(double start, double stop, int ticksCount)
        {
            if (Coeffs == null || Coeffs.Length == 0 || Scale == null || ticksCount == 0)
                return null;

            DebugTraceLog.WriteLine("MinorTick --> GetTicks() : ticksCount=" + ticksCount.ToString());

            double step = (stop - start) / (ticksCount + 1);
            List<TickInfo> res = new List<TickInfo>();

            int diff = Coeffs.Length - ticksCount;
            int startIndex = diff / 2;
            for (int i = 0; i < ticksCount && startIndex < Coeffs.Length; i++)
            {
                var val = start + step * (i + 1);
                if (ValidMinorRange(val))
                {
                    res.Add(new TickInfo(val, Scale.ToPixels(val), false, Coeffs[startIndex] * mayorTickLength));
                }
                startIndex++;
            }
            return res.ToArray();
        }

        private bool ValidMinorRange(double val)
        {
            return val >= this.mayorRangeStart && val <= this.mayorRangeStop;
        }

        public int DecreaseTickCount(int tickCount)
        {
            return tickCount > 2 ? (tickCount - 2) : 0;
        }

        public int IncreaseTickCount(int tickCount)
        {
            return ((tickCount + 2) < Coeffs.Length) ?  (tickCount + 2) : Coeffs.Length;
        }

        public TickInfo[] GetTicks(int ticksCount)
        {
            List<TickInfo> list = new List<TickInfo>();
            if (ranges != null && ranges.Length > 0 && ticksCount != 0)
            {
                foreach (var r in ranges)
                {
                    var t = GetTicks(r.Min, r.Max, ticksCount);
                    if (t != null && t.Length > 0)
                    {
                        list.AddRange(t);
                    }
                }
                return list.ToArray();
            }
            else
            {
                return list.ToArray();
            }
        }

        public int DefaultTickCount()
        {
            return (Coeffs != null && Coeffs.Length > 0) ? Coeffs.Length : 10;
        }

        public ITicksCreater MinorTickCreater
        {
            get { return null; }
        }
    }    
}
