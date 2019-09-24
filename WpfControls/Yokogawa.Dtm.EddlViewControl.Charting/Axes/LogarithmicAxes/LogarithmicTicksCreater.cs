using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicTicksCreater : ITicksCreater<double>
    {
        private Scale scale = null;
        public Scale Scale
        {
            get { return scale; }
            set { scale = value; MinorLogarithmicTicksCreater.Scale = scale; }
        }

        private double mayorTickLength = 10;
        public double MayorTickLength
        {
            get { return mayorTickLength; }
            set { mayorTickLength = value; }
        }

        private LogarithmicAxisTicks tickMask = LogarithmicAxisTicks.All;
        public LogarithmicAxisTicks TickMask
        {
            get { return tickMask; }
            set { tickMask = value; }
        }

        protected double mayorRangeStart;
        protected double mayorRangeStop;
        public virtual void SetRange(double start, double stop)
        {
            this.mayorRangeStart = start;
            this.mayorRangeStop = stop;
            MinorLogarithmicTicksCreater.SetRange(start, stop);
        }

        public virtual TickInfo[] GetTicks(double start, double stop, int ticksCount)
        {
            return GetTicks(start, stop, ticksCount, true, false);
        }

        protected TickInfo[] GetTicks(double start, double stop, int ticksCount, bool createMayorTick, bool createMinorTick)
        {
            List<TickInfo> retTicks = new List<TickInfo>();

            if (start <= 0.0 || stop <= 0.0 || start == stop)
            {
                return retTicks.ToArray();
            }

            int mask = (int)TickMask;

            double tickPos = AdjacentTick(start, true);
            if (tickPos <= stop)
            {

                double power;
                int tick = DecimateTick(tickPos, out power);

                while (tickPos <= stop)
                {
                    if (tick == 1)
                    {
                        if (createMayorTick)
                        {
                            retTicks.Add(new TickInfo(tickPos, this.Scale.ToPixels(tickPos), true, MayorTickLength));
                        }
                    }
                    else if (((1 << tick - 2) & mask) != 0)
                    {
                        if (createMinorTick)
                        {
                            retTicks.Add(new TickInfo(tickPos, this.Scale.ToPixels(tickPos), false, MayorTickLength / 2));
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
            return retTicks.ToArray();
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

        public virtual TickInfo[] GetTicks(int ticksCount)
        {
            return GetTicks(mayorRangeStart, mayorRangeStop, ticksCount);
        }

        public int DecreaseTickCount(int tickCount)
        {
            return tickCount;
        }

        public int IncreaseTickCount(int tickCount)
        {
            return tickCount;
        }

        public int DefaultTickCount()
        {
            return 10;
        }

        private MinorLogarithmicTicksCreater minorLogarithmicTicksCreater = null;
        public MinorLogarithmicTicksCreater MinorLogarithmicTicksCreater
        {
            get
            {
                if (minorLogarithmicTicksCreater == null)
                {
                    minorLogarithmicTicksCreater = new MinorLogarithmicTicksCreater();
                }
                return minorLogarithmicTicksCreater; 
            }
        }
        public ITicksCreater MinorTickCreater
        {
            get 
            {
                return MinorLogarithmicTicksCreater;
            }
        }
    }

    public class MinorLogarithmicTicksCreater : ITicksCreater<double>
    {
        private Scale scale = null;
        public Scale Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private double mayorTickLength = 10;
        public double MayorTickLength
        {
            get { return mayorTickLength; }
            set { mayorTickLength = value; }
        }

        private LogarithmicAxisTicks tickMask = LogarithmicAxisTicks.All;
        public LogarithmicAxisTicks TickMask
        {
            get { return tickMask; }
            set { tickMask = value; }
        }

        protected double mayorRangeStart;
        protected double mayorRangeStop;
        public virtual void SetRange(double start, double stop)
        {
            this.mayorRangeStart = start;
            this.mayorRangeStop = stop;
        }

        public virtual TickInfo[] GetTicks(double start, double stop, int ticksCount)
        {
            return GetTicks(start, stop, ticksCount, false, true);
        }

        protected TickInfo[] GetTicks(double start, double stop, int ticksCount, bool createMayorTick, bool createMinorTick)
        {
            List<TickInfo> retTicks = new List<TickInfo>();

            if (start <= 0.0 || stop <= 0.0 || start == stop)
            {
                return retTicks.ToArray();
            }

            int mask = (int)TickMask;

            double tickPos = AdjacentTick(start, true);
            if (tickPos <= stop)
            {

                double power;
                int tick = DecimateTick(tickPos, out power);

                while (tickPos <= stop)
                {
                    if (tick == 1)
                    {
                        if (createMayorTick)
                        {
                            retTicks.Add(new TickInfo(tickPos, this.Scale.ToPixels(tickPos), true, MayorTickLength));
                        }
                    }
                    else if (((1 << tick - 2) & mask) != 0)
                    {
                        if (createMinorTick)
                        {
                            retTicks.Add(new TickInfo(tickPos, this.Scale.ToPixels(tickPos), false, MayorTickLength * 3 / 4));
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
            return retTicks.ToArray();
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

        public virtual TickInfo[] GetTicks(int ticksCount)
        {
            return GetTicks(mayorRangeStart, mayorRangeStop, ticksCount);
        }

        public int DecreaseTickCount(int tickCount)
        {
            return tickCount;
        }

        public int IncreaseTickCount(int tickCount)
        {
            return tickCount;
        }

        public int DefaultTickCount()
        {
            return 10;
        }
        public ITicksCreater MinorTickCreater
        {
            get
            {
                return null;
            }
        }
    }

}
