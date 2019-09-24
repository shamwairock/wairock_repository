using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicScale : Scale
    {
        private double scale = 1.0;
        public double Scale
        {
            get { return scale; }
        }

        double start = 1.0;
        public override double Start
        {
            get { return start; }
        }

        double stop = 1000.0;
        public override double Stop
        {
            get { return stop; }
        }

        public override bool IsConsistent
        {
            get
            {
                if (!base.IsConsistent)
                    return false;
                try
                {
                    double start = Convert.ToDouble(Start);
                    if (double.IsNaN(start) || double.IsInfinity(start))
                        return false;
                    double stop = Convert.ToDouble(Stop);
                    if (double.IsNaN(stop) || double.IsInfinity(stop))
                        return false;
                    if (start > 0.0 && stop > 0.0 && start != stop)
                        return true;
                    return false;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }
        }

        public override int TickCount
        {
            get { return 0; }
            set { }
        }

        public override double MinDefaultRange
        {
            get { return 1.0; }
        }

        public override double MaxDefaultRange
        {
            get { return 1000.0; }
        }

        LogarithmicAxisTicks tickMask = LogarithmicAxisTicks.All;
        public LogarithmicAxisTicks TickMask
        {
            get { return tickMask; }
            set { tickMask = value; }
        }

        public override double ToPixels(double value)
        {
            return this.StartPixels + Math.Log10(value / this.Start) * this.Scale;
        }

        public override double FromPixels(double pixels)
        {
            pixels = pixels - this.startPixels;
            return this.Start * Math.Pow(10, pixels / this.Scale);
        }

        public override IEnumerable<TickInfo> Ticks()
        {
            double start = Convert.ToDouble(Start), stop = Convert.ToDouble(Stop);
            if (start <= 0.0 || stop <= 0.0 || start == stop)
                yield break;
            int mask = (int)TickMask;

            if (start < stop)
            {
                double tickPos = AdjacentTick(start, true);
                if (tickPos > stop)
                    yield break;

                double power;
                int tick = DecimateTick(tickPos, out power);

                while (tickPos <= stop)
                {
                    if (tick == 1)
                        yield return new TickInfo(tickPos, this.ToPixels(tickPos), true);
                    else if (((1 << tick - 2) & mask) != 0)
                        yield return new TickInfo(tickPos, this.ToPixels(tickPos), false);

                    tick++;
                    if (tick == 10)
                    {
                        tick = 1;
                        power *= 10;
                    }
                    tickPos = tick * power;
                }
            }
            else // startValue > stopValue
            {
                double tickPos = AdjacentTick(start, false);
                if (tickPos < stop)
                    yield break;
                double power;
                int tick = DecimateTick(tickPos, out power);

                while (tickPos >= stop)
                {
                    if (tick == 1)
                        yield return new TickInfo(tickPos, this.ToPixels(tickPos), true);
                    else if (((1 << tick - 2) & mask) != 0)
                        yield return new TickInfo(tickPos, this.ToPixels(tickPos), false);

                    tick--;
                    if (tick == 0)
                    {
                        tick = 9;
                        power /= 10;
                    }
                    tickPos = tick * power;
                }
            }
        }

        private static int DecimateTick(double tick, out double power)
        {
            double log = Math.Log10(tick);
            int n = (int)log;
            power = Math.Pow(10, n);
            return (int)(tick / power);
        }

        private static double AdjacentTick(double value, bool bNext)
        {
            if (value == 1.0)
                return 1.0;

            // Present pixels as a*10^n where n - any intger; 1.0 <= a < 10.0
            double log = Math.Log10(value);
            int n = (int)log;
            if (log < 0.0)
                n--;
            double power = Math.Pow(10, n);
            double a = value / power;
            int tick = (int)a;
            if (tick * power == value)
                return value;
            if (bNext)
                return (tick + 1) * power;
            else
                return tick * power;
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

        void AutoArrage(double start, double stop, double startPixels, double stopPixels, bool autoRange)
        {
            if (IsValidRangeData(start, stop) &&
                !double.IsNaN(startPixels) && !double.IsInfinity(startPixels) &&
                !double.IsNaN(startPixels) && !double.IsInfinity(startPixels) && 
                (stopPixels - startPixels) > 0.0)
            {
                bool minMaxDirection = start < stop;

                double ss = start < stop ? start : stop;

                double ee = start > stop ? start : stop;

                if (ee > 0 && ss <= 0)
                {
                    ss = CalculateLogarithmicRangeValue(ee, false);
                }

                double graphmin = Math.Pow(10, (int)Math.Floor(Math.Log10(ss)));

                double graphmax = Math.Pow(10, (int)Math.Ceiling(Math.Log10(ee)));

                if (autoRange)
                {
                    this.start = minMaxDirection ? graphmin : graphmax;
                    this.stop = minMaxDirection ? graphmax : graphmin;
                }
                else
                {
                    this.start = minMaxDirection ? ss : ee;
                    this.stop = minMaxDirection ? ee : ss;
                }
            }

            this.startPixels = startPixels;
            this.stopPixels = stopPixels;

            var extent = this.stopPixels - this.startPixels;
            this.scale = extent / Math.Log10(this.Stop / this.Start);
        }

        public override bool IsValidRangeData(double start, double stop)
        {
            if (double.IsNaN(start) || double.IsNaN(stop)
                || (start <= 0 && stop <= 0)
                || double.IsPositiveInfinity(start)
                || double.IsPositiveInfinity(stop)
                || start == stop)
            {
                return false;
            }
            return true;
        }

        public override void GetValidRangeData(ref double ss, ref double ee, bool autoRange)
        {
            if (double.IsNaN(ss) || double.IsNaN(ee)|| double.IsPositiveInfinity(ss) || double.IsPositiveInfinity(ee))
            {
                ss = this.MinDefaultRange;
                ee = this.MaxDefaultRange;
            }
            //else if (ss <= 0 && ee <= 0)
            //{
            //    ss = this.MinDefaultRange;
            //    ee = this.MaxDefaultRange;
            //}
            //else if (ee > 0 && ss <= 0)
            //{
            //    ss = CalculateLogarithmicRangeValue(ee, ss <= ee);
            //}
            //else if (ss > 0 && ee <= 0)
            //{
            //    ee = CalculateLogarithmicRangeValue(ss, ss > ee);
            //}
            if (ss == ee)
            {
                if (autoRange)
                {
                    ee = ss + 1.0;
                }
                else
                {
                    ss = this.MinDefaultRange;
                    ee = this.MaxDefaultRange;
                }
            }
        }

        private double CalculateLogarithmicRangeValue(double value, bool calculateMax)
        {
            if (calculateMax)
            {
               return Math.Pow(10, Math.Ceiling(Math.Log10(value) + 1));
            }
            else
            {
                return Math.Pow(10, Math.Floor(Math.Log10(value) - 1));
            }
        }

        public override IList<TickInfo> GetTicks(int tickCount)
        {
            return GetTicks(tickCount, true, false);
        }

        public override IList<TickInfo> GetMinorTicks(int tickCount, int minorTicksCount)
        {
            return GetTicks(tickCount, false, true);
        }

        private IList<TickInfo> GetTicks(int tickCount, bool createLongTick, bool createMinorTick)
        {
            List<TickInfo> retTicks = new List<TickInfo>();

            double start = Convert.ToDouble(Start);
            double stop = Convert.ToDouble(Stop);

            if (start <= 0.0 || stop <= 0.0 || start == stop)
            {
                return retTicks;
            }

            int mask = (int)TickMask;

            if (start < stop)
            {
                double tickPos = AdjacentTick(start, true);
                if (tickPos <= stop)
                {

                    double power;
                    int tick = DecimateTick(tickPos, out power);

                    while (tickPos <= stop)
                    {
                        if (tick == 1)
                        {
                            if (createLongTick)
                            {
                                retTicks.Add(new TickInfo(tickPos, this.ToPixels(tickPos), true));
                            }
                        }
                        else if (((1 << tick - 2) & mask) != 0)
                        {
                            if (createMinorTick)
                            {
                                retTicks.Add(new TickInfo(tickPos, this.ToPixels(tickPos), false));
                            }
                        }
                        tick++;
                        if (tick == 10)
                        {
                            tick = 1;
                            power *= 10;
                        }
                        tickPos = tick * power;
                    }
                }
            }
            else // startValue > stopValue
            {
                double tickPos = AdjacentTick(start, false);
                if (tickPos >= stop)
                {
                    double power;
                    int tick = DecimateTick(tickPos, out power);

                    while (tickPos >= stop)
                    {
                        if (tick == 1)
                        {
                            if (createLongTick)
                            {
                                retTicks.Add(new TickInfo(tickPos, this.ToPixels(tickPos), true));
                            }
                        }
                        else if (((1 << tick - 2) & mask) != 0)
                        {
                            if (createMinorTick)
                            {
                                retTicks.Add(new TickInfo(tickPos, this.ToPixels(tickPos), false));
                            }
                        }

                        tick--;
                        if (tick == 0)
                        {
                            tick = 9;
                            power /= 10;
                        }
                        tickPos = tick * power;
                    }
                }
            }
            return retTicks;
        }
    }

}
