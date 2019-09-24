using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearScale : Scale
    {
        private double scale = 0.0;
        public double Scale
        {
            get { return scale; }
        }

        double start;
        public override double Start
        {
            get { return start; }
        }

        double stop = 100.0;
        public override double Stop
        {
            get { return stop; }
        }

        public override bool IsConsistent
        {
            get
            {
                if (!base.IsConsistent || TickCount <= 0)
                    return false;
                try
                {
                    double start = Convert.ToDouble(Start);
                    if (double.IsNaN(start) || double.IsInfinity(start))
                        return false;
                    double stop = Convert.ToDouble(Stop);
                    if (double.IsNaN(stop) || double.IsInfinity(stop))
                        return false;
                    if (start == stop)
                        return false;
                    return true;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }
        }

        int minorTickCount = 4;
        public virtual int MinorTickCount
        {
            get { return minorTickCount; }
            set { minorTickCount = value; }
        }

        int tickCount = 6;
        public override int TickCount
        {
            get { return tickCount; }
            set { tickCount = value; }
        }

        double longTickStep = 1.0;
        public virtual double LongTickStep
        {
            get { return longTickStep; }
        }

        public override double MinDefaultRange
        {
            get { return 0.0; }
        }

        public override double MaxDefaultRange
        {
            get { return 100.0; }
        }

        private bool showMinorTick = true;
        public bool ShowMinorTick
        {
            get { return showMinorTick; }
            set { showMinorTick = value; }
        }

        public override void SetScale(double start, double stop, double startPixels, double stopPixels, bool autoRange)
        {
            AutoArrage(start, stop, startPixels, stopPixels, autoRange);
        }

        private double startPixels = 0;
        public double StartPixels
        {
            get { return startPixels; }
        }

        private double stopPixels = 0;
        public double StopPixels
        {
            get { return stopPixels; }
        }

        private void AutoArrage(double startValue, double stopValue, double startPixels, double stopPixels, bool autoRange)
        {
            if (startValue == stopValue || this.TickCount <= 1)
            {
                return;
            }

            bool minMaxDirection = startValue < stopValue;

            double ss = startValue < stopValue ? startValue : stopValue;

            double ee = startValue > stopValue ? startValue : stopValue;

            double range = NiceNum(ee - ss, false);

            double d = NiceNum(range / (tickCount - 1), true);

            double graphmin = Math.Floor(ss / d) * d;

            double graphmax = Math.Ceiling(ee / d) * d;

            if (autoRange)
            {
                this.start = minMaxDirection ? graphmin : graphmax;
                this.stop = minMaxDirection ? graphmax : graphmin;
            }
            else
            {
                this.start = startValue;
                this.stop = stopValue;
            }

            this.longTickStep = d; 

            this.startPixels = startPixels;
            this.stopPixels = stopPixels;
            var extent = this.stopPixels - this.startPixels;
            this.scale = extent / Math.Abs(this.Stop - this.Start);

            //System.Diagnostics.Debug.WriteLine("**");
            //System.Diagnostics.Debug.WriteLine(string.Format("StartPixels = {0}", this.StartPixels));
            //System.Diagnostics.Debug.WriteLine(string.Format("StopPixels = {0}", this.StopPixels));
            //System.Diagnostics.Debug.WriteLine(string.Format("extent = {0}", extent));
            //System.Diagnostics.Debug.WriteLine(string.Format("Scale = {0}", this.Scale));
        }

        public override double ToPixels(double value)
        {
            return (value - this.Start) * this.Scale + this.StartPixels;
        }

        public override double FromPixels(double pixels)
        {
            pixels = pixels - this.StartPixels;
            return this.Scale == 0 ? 0 : (pixels / this.Scale + Start);
        }

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

        private double[] ticks;
        public override IEnumerable<TickInfo> Ticks()
        {
            if (start == stop)
                yield break;
#if true

            double ss = start < stop ? start : stop;

            double ee = start > stop ? start : stop;

            double delta = ee - ss;

            int log = (int)Math.Round(Math.Log10(delta));

            double newStart = RoundHelper.Round(ss, log);

            double newStop = RoundHelper.Round(ee, log);

            if (newStart == newStop)
            {
                log--;
                newStart = RoundHelper.Round(ss, log);
                newStop = RoundHelper.Round(newStop, log);
            }

            // calculating step between ticks
            double unroundedStep = (newStop - newStart) / tickCount;
            int stepLog = log;
            // trying to round step
            double step = RoundHelper.Round(unroundedStep, stepLog);
            if (step == 0)
            {
                stepLog--;
                step = RoundHelper.Round(unroundedStep, stepLog);
                if (step == 0)
                {
                    // step will not be rounded if attempts to be rounded to zero.
                    step = unroundedStep;
                }
            }

            if (step != 0.0)
            {
                ticks = CreateTicks(ss, ee, step);
            }
            else
            {
                ticks = new double[] { };
            }

            int index = 0;
            if (start < stop)
            {
                while (index < ticks.Length )
                {
                    if (ticks[index] >= start && ticks[index] <= stop)
                    {
                        var value = ticks[index];
                        var pos = this.ToPixels(value);

                        yield return new TickInfo(value, pos, true);
                    }
                    index++;
                }
            }
            else
            {
                while (index < ticks.Length)
                {
                    if(ticks[index] <= start && ticks[index] >= stop)
                    {
                        var value = ticks[index];
                        var pos = this.ToPixels(value);

                        yield return new TickInfo(value, pos, true);
                    }
                    index++;
                }
            }
#else
            



            double longTickStep = this.LongTickStep;

            double longTickAnchor = NearestLongTick();

            double longTickPos = longTickAnchor;

            int longTickProcessed = 0;

            int minorTickCount = this.MinorTickCount;
            double minorTikcStep = longTickStep / (minorTickCount + 1);

            double minorTickPos;

            if (start < stop)
            {
                while (longTickPos <= stop)
                {
                    if (longTickPos >= start)
                    {
                        var val = longTickPos;
                        var pos = this.ToPixels(longTickPos, isRevert);

                        yield return new TickInfo(val, pos, true);
                    }

                    if (this.ShowMinorTick)
                    {
                        for (int i = 1; i <= minorTickCount; i++)
                        {
                            minorTickPos = longTickPos + minorTikcStep * i;
                            if (minorTickPos >= start && minorTickPos <= stop)
                            {
                                yield return new TickInfo(minorTickPos, this.ToPixels(minorTickPos, isRevert), false);
                            }
                        }
                    }

                    longTickPos = longTickAnchor + (++longTickProcessed) * longTickStep;
                }
            }
            else // start > stop
            {
                while (longTickPos >= stop)
                {
                    if (longTickPos <= start)
                    {
                        yield return new TickInfo(longTickPos, this.ToPixels(longTickPos, isRevert), true);
                    }

                    if (this.ShowMinorTick)
                    {
                        for (int i = 1; i <= minorTickCount; i++)
                        {
                            minorTickPos = longTickPos - minorTikcStep * i;
                            if (minorTickPos <= start && minorTickPos >= stop)
                            {
                                yield return new TickInfo(minorTickPos, this.ToPixels(minorTickPos, isRevert), false);
                            }
                        }
                    }

                    longTickPos = longTickAnchor - (++longTickProcessed) * longTickStep;
                }
            }

#endif
            
        }

        protected double FirstTick()
        {
            double ss = this.Start;
            double firstLongTick = NearestLongTick();

            if (ss == firstLongTick)
            {
                return firstLongTick;
            }

            double anchor = ss;

            double minorTickStep = this.longTickStep / (this.MinorTickCount + 1.0);

            int n = (int)(Math.Abs(ss - firstLongTick) / minorTickStep);

            if (ss < firstLongTick)
            {
                anchor = firstLongTick - minorTickStep * n;
            }
            else
            {
                anchor = firstLongTick + minorTickStep * n;
            }
            return anchor;
        }

        protected double NearestLongTick()
        {
            double ss = this.Start;
            double ee = this.Stop;

            if (ss == ee)
            {
                return double.NaN;
            }

            double anchor = ss;
            if (ss > ee)
            {
                int n = (int)Math.Ceiling((ss - ee) / this.LongTickStep);
                anchor = ee + longTickStep * n;
            }
            return anchor;
        }

        public static double NiceNum(double x, bool round)
        {
            int expv;				/* exponent of x */
            double f;				/* fractional part of x */
            double nf;				/* nice, rounded fraction */

            expv = (int)Math.Floor(Math.Log10(x));
            f = x / Math.Pow(10.0, expv);		    /* between 1 and 10 */
            if (round)
            {
                if (f < 1.5)
                {
                    nf = 1.0;
                }
                else if (f < 3.0)
                {
                    nf = 2.0;
                }
                else if (f < 7.0)
                {
                    nf = 5.0;
                }
                else
                {
                    nf = 10.0;
                }
            }
            else
            {
                if (f <= 1.0)
                {
                    nf = 1.0;
                }
                else if (f <= 2.0)
                {
                    nf = 2.0;
                }
                else if (f <= 5.0)
                {
                    nf = 5.0;
                }
                else
                {
                    nf = 10.0;
                }
            }
            return nf * Math.Pow(10.0, expv);
        }

        public override bool IsValidRangeData(double start, double stop)
        {
            if (double.IsNaN(start) || (double.IsNaN(stop)) || double.IsInfinity(start) || double.IsInfinity(stop) || start == stop)
            {
                return false;
            }
            return true;
        }

        public override void GetValidRangeData(ref double ss, ref double ee, bool autoRange)
        {
            if (double.IsNaN(ss) || (double.IsNaN(ee)) || double.IsInfinity(ss) || double.IsInfinity(ee))
            {
                ss = this.MinDefaultRange;
                ee = this.MaxDefaultRange;
            }
            else if (ss == ee)
            {
                if (autoRange)
                {
                    ss = ss - 1.0;
                    ee = ss + 2.0;
                }
                else
                {
                    ss = this.MinDefaultRange;
                    ee = this.MaxDefaultRange;
                }
            }
        }

        protected virtual double CalculateTickStep(double ss, double ee, int count)
        {
            double delta = ee - ss;

            int log = (int)Math.Round(Math.Log10(delta));

            double newStart = RoundHelper.Round(ss, log);

            double newStop = RoundHelper.Round(ee, log);

            if (newStart == newStop)
            {
                log--;
                newStart = RoundHelper.Round(ss, log);
                newStop = RoundHelper.Round(newStop, log);
            }

            if (newStop < newStart)
            {
                var t = newStart;
                newStart = newStop;
                newStop = t;
            }

            // calculating step between ticks
            double unroundedStep = (newStop - newStart) / count;
            int stepLog = log;
            // trying to round step
            double step = RoundHelper.Round(unroundedStep, stepLog);
            if (step == 0)
            {
                stepLog--;
                step = RoundHelper.Round(unroundedStep, stepLog);
                if (step == 0)
                {
                    // step will not be rounded if attempts to be rounded to zero.
                    step = unroundedStep;
                }
            }

            return step;
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

        private IList<TickInfo> CraeteMayorAndMinorTicks(int mayorTicksCount, int minorTicksCount, bool createMayorTick, bool createMinorTick)
        {
            List<TickInfo> retTicks = new List<TickInfo>();

            if (start == stop)
                return retTicks;

#if true
            double ss = start < stop ? start : stop;

            double ee = start > stop ? start : stop;

            double step = CalculateTickStep(ss, ee, mayorTicksCount);
            step = UpdateMayorInterval(step, this.MinMayorTickInterval, this.MaxMayorTickInterval);
            if (step != 0.0)
            {
                ticks = CreateTicks(ss, ee, step);
            }
            else
            {
                ticks = new double[] { };
            }

            int index = 0;
            if (start < stop)
            {
                while (index < ticks.Length)
                {
                    if (createMayorTick && ticks[index] >= start && ticks[index] <= stop)
                    {
                        retTicks.Add(new TickInfo(ticks[index], this.ToPixels(ticks[index]), true));
                    }

                    if (createMinorTick && this.ShowMinorTick && index > 0)
                    {
                        double minorStep = CalculateTickStep(ticks[index - 1], ticks[index], minorTicksCount);
                        var minorTicks = CreateTicks(ticks[index - 1], ticks[index], minorStep);

                        int size = minorTicks.Count();
                        for (int i = 1; i < size - 1; i++)
                        {
                            var mTick = minorTicks[i];
                            if (mTick >= start && mTick <= stop)
                            {
                                retTicks.Add(new TickInfo(mTick, this.ToPixels(mTick), false));
                            }
                        }

                    }   
                    index++;
                }
            }
            else
            {
                while (index < ticks.Length)
                {
                    if (createMayorTick && ticks[index] <= start && ticks[index] >= stop)
                    {
                        retTicks.Add(new TickInfo(ticks[index], this.ToPixels(ticks[index]), true));
                    }
                    if (createMinorTick && this.ShowMinorTick && index > 0)
                    {
                        double minorStep = CalculateTickStep(ticks[index - 1], ticks[index], minorTicksCount);
                        var minorTicks = CreateTicks(ticks[index - 1], ticks[index], minorStep);
                        int size = minorTicks.Count();
                        for (int i = 1; i < size - 1; i++)
                        {
                            var mTick = minorTicks[i];
                            if (mTick <= start && mTick >= stop)
                            {
                                retTicks.Add(new TickInfo(mTick, this.ToPixels(mTick), false));
                            }
                        }
                    }  
                    index++;
                }
            }
#else
            
            double longTickStep = this.LongTickStep;

            double longTickAnchor = NearestLongTick();

            double longTickPos = longTickAnchor;

            int longTickProcessed = 0;

            int minorTickCount = this.MinorTickCount;
            double minorTikcStep = longTickStep / (minorTickCount + 1);

            double minorTickPos;

            if (start < stop)
            {
                while (longTickPos <= stop)
                {
                    if (longTickPos >= start)
                    {
                        var val = longTickPos;
                        var pos = this.ToPixels(longTickPos, isRevert);

                        yield return new TickInfo(val, pos, true);
                    }

                    if (this.ShowMinorTick)
                    {
                        for (int i = 1; i <= minorTickCount; i++)
                        {
                            minorTickPos = longTickPos + minorTikcStep * i;
                            if (minorTickPos >= start && minorTickPos <= stop)
                            {
                                yield return new TickInfo(minorTickPos, this.ToPixels(minorTickPos, isRevert), false);
                            }
                        }
                    }

                    longTickPos = longTickAnchor + (++longTickProcessed) * longTickStep;
                }
            }
            else // start > stop
            {
                while (longTickPos >= stop)
                {
                    if (longTickPos <= start)
                    {
                        yield return new TickInfo(longTickPos, this.ToPixels(longTickPos, isRevert), true);
                    }

                    if (this.ShowMinorTick)
                    {
                        for (int i = 1; i <= minorTickCount; i++)
                        {
                            minorTickPos = longTickPos - minorTikcStep * i;
                            if (minorTickPos <= start && minorTickPos >= stop)
                            {
                                yield return new TickInfo(minorTickPos, this.ToPixels(minorTickPos, isRevert), false);
                            }
                        }
                    }

                    longTickPos = longTickAnchor - (++longTickProcessed) * longTickStep;
                }
            }

#endif

            return retTicks;
        }

        

        public override IList<TickInfo> GetTicks(int tickCount)
        {
            return CraeteMayorAndMinorTicks(tickCount, 0, true, false);
        }

        public override IList<TickInfo> GetMinorTicks(int tickCount, int minorTicksCount)
        {
            return CraeteMayorAndMinorTicks(tickCount, minorTicksCount, false, true);
        }
    }

}
