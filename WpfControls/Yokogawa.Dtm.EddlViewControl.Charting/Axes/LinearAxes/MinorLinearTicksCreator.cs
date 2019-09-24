using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class MinorLinearTicksCreator
    {
        private Range<double>[] ticksRanges;
        private Range<double> axisRange;
        private int tickCount = 3;
        internal void SetRanges(Range<double>[] ranges, Range<double> range)
        {
            ticksRanges = ranges;
            axisRange = range;
            tickCount = 3;
        }

        public MinorLinearTicksCreator()
        {
            Coeffs = new double[] { 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6 };
        }

        public MinorLinearTicksCreator(double[] coeffs)
        {
            if (coeffs != null && coeffs.Length > 0)
            {
                Coeffs = coeffs;
            }
        }

        public double[] Coeffs { get; set; }

        public double[] GetTicks(double start, double stop, int ticksCount)
        {
            if (Coeffs == null || Coeffs.Length == 0 || ticksCount == 0)
                return null;

            double step = (stop - start) / (ticksCount + 1);
            List<double> res = new List<double>();

            int diff = Coeffs.Length - ticksCount;
            int startIndex = diff / 2;
            for (int i = 0; i < ticksCount && startIndex < Coeffs.Length; i++)
            {
                var val = start + step * (i + 1);
                if (ValidMinorRange(val))
                {
                    res.Add(val);
                }
                startIndex++;
            }
            return res.ToArray();
        }

        private bool ValidMinorRange(double val)
        {
            return val >= axisRange.Min && val <= axisRange.Max;
        }

        public void DecreaseTickCount()
        {
            tickCount = tickCount > 2 ? (tickCount - 2) : 0;
        }

        public void IncreaseTickCount()
        {
            tickCount = ((tickCount + 2) < Coeffs.Length) ? (tickCount + 2) : Coeffs.Length;
        }

        public double[] CreateTicks()
        {
            List<double> list = new List<double>();
            if (ticksRanges != null && ticksRanges.Length > 0 && tickCount != 0)
            {
                foreach (var r in ticksRanges)
                {
                    var t = GetTicks(r.Min, r.Max, tickCount);
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
    }

    public class MinorLinearTicksCalculator
    {
        private int tickCount = 3;
        public void Initialize()
        {
            tickCount = 3;
        }

        public MinorLinearTicksCalculator()
        {
            Coeffs = new double[] { 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6 };
        }

        public MinorLinearTicksCalculator(double[] coeffs)
        {
            if (coeffs != null && coeffs.Length > 0)
            {
                Coeffs = coeffs;
            }
        }

        public double[] Coeffs { get; set; }

        public void DecreaseTickCount()
        {
            tickCount = tickCount > 2 ? (tickCount - 2) : 0;
        }

        public void IncreaseTickCount()
        {
            tickCount = ((tickCount + 2) < Coeffs.Length) ? (tickCount + 2) : Coeffs.Length;
        }

        public double[] CreateTicks(Range<double>[] ticksRanges, Range<double> axisRange)
        {
            List<double> list = new List<double>();
            if (ticksRanges != null && ticksRanges.Length > 0 && tickCount != 0)
            {
                foreach (var r in ticksRanges)
                {
                    var t = GetTicks(r.Min, r.Max, axisRange.Min, axisRange.Max, tickCount);
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

        private double[] GetTicks(double start, double stop, double min, double max, int ticksCount)
        {
            if (Coeffs == null || Coeffs.Length == 0 || ticksCount == 0)
                return null;

            double step = (stop - start) / (ticksCount + 1);
            List<double> res = new List<double>();

            int diff = Coeffs.Length - ticksCount;
            int startIndex = diff / 2;
            for (int i = 0; i < ticksCount && startIndex < Coeffs.Length; i++)
            {
                var val = start + step * (i + 1);
                if (val >= min && val <= max)
                {
                    res.Add(val);
                }
                startIndex++;
            }
            return res.ToArray();
        }
    }
}
