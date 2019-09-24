using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class LogarithmicTickCalculator
    {
        private Range<double> valueRange;
        public void SetRange(Range<double> range)
        {
            valueRange = range;
        }

        public LogarithmicValue[] CreateTicks(bool only_inside = true)
        {
            List<LogarithmicValue> ret = new List<LogarithmicValue>();

            var tickMinPos = AdjacentTick(valueRange.Min, false);
            var tickMaxPos = AdjacentTick(valueRange.Max, true);

            var tickPos = tickMinPos;

            double tickMin = only_inside ? valueRange.Min : tickMinPos.Value;
            double tickMax = only_inside ? valueRange.Max : tickMaxPos.Value;

            int n;
            int tick = DecimateTick(tickPos.Value, out n);
            while (tickPos.Value <= tickMaxPos.Value)
            {
                if (tickPos.Value >= tickMin && tickPos.Value <= tickMax)
                {
                    ret.Add(tickPos);
                }
                tick++;
                if (tick == 10)
                {
                    tick = 1;
                    n++;
                }
                tickPos = new LogarithmicValue(tick, n);
            }
            return ret.ToArray();
        }

        public static int DecimateTick(double tick, out int n)
        {
            double log = Math.Log10(tick);
            n = (int)log;
            var power = Math.Pow(10, n);
            return (int)(tick / power);
        }

        public static LogarithmicValue AdjacentTick(double value, bool bNext)
        {
            if (value == 1.0)
            {
                return new LogarithmicValue(1.0, 0);
            }

            // Present pixels as a*10^n where n - any intger; 1.0 <= a < 10.0
            double log = Math.Log10(value);
            int n = (int)log;
            if (log < 0.0)
            {
                n--;
            }
            double power = Math.Pow(10, n);
            double a = value / power;
            int tick = (int)a;
            if (tick * power == value)
            {
                return new LogarithmicValue(tick, n);
            }
            if (bNext)
            {
                return new LogarithmicValue(tick + 1, n);
            }
            else
            {
                return new LogarithmicValue(tick, n);
            }
        }
    }
}
