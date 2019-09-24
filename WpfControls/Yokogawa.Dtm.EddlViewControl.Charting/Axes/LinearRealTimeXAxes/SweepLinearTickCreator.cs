using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class SweepLinearTickCreator
    {
        private LinearTickCalculator tickCalculator = new LinearTickCalculator();

        public double[] CreateTicks(bool only_inside = true)
        {
            var currentInterval = tickCalculator.CalculateInterval();
            if (minInterval.HasValue && currentInterval < minInterval.Value)
            {
                return null;
            }
            if (maxInterval.HasValue && currentInterval > maxInterval.Value)
            {
                return null;
            }
            return tickCalculator.CreateTicks(range, only_inside);
        }

        public bool DescentTickCount()
        {
            tickCalculator.DescentTickCount();
            var currentInterval = tickCalculator.CalculateInterval();
            if (maxInterval.HasValue && currentInterval > maxInterval.Value)
            {
                tickCalculator.IncreaseTickCount();
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IncreaseTickCount()
        {
            tickCalculator.IncreaseTickCount();
            var currentInterval = tickCalculator.CalculateInterval();
            if (minInterval.HasValue && currentInterval < minInterval.Value)
            { 
                tickCalculator.DescentTickCount();
                return false;
            }
            else
            {
                return true;
            }
        }

        private Range<double> range;
        private double? minInterval;
        private double? maxInterval;
        public void SetRange(Range<double> range, double? minInterval, double? maxInterval)
        {
            this.range = range;
            this.minInterval = minInterval;
            this.maxInterval = maxInterval;
            tickCalculator.Initialize(range);
        }

        private MinorLinearTicksCreator minorTickCreator = new MinorLinearTicksCreator();
        public void InitializeMinorTickCreator()
        {
            var ticks = CreateTicks(false);

            var ranges = new List<Range<double>>();
            if (ticks.Length > 0)
            {
                for (int i = 0; i < (ticks.Length - 1); i++)
                {
                    ranges.Add(new Range<double>(ticks[i], ticks[i + 1]));
                }
            }
            minorTickCreator.SetRanges(ranges.ToArray(), range);
        }

        public MinorLinearTicksCreator GetMinorLinearTicksCreator()
        {
            return minorTickCreator;
        }
    }
}
