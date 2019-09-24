using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class LinearTickCalculator
    {
        private int delta = 1;
        public int Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        private int beta = 0;
        public int Beta
        {
            get { return beta; }
            set { beta = value; }
        }

        public double CalculateInterval()
        {
            return delta * Math.Pow(10, beta);
        }

        public void DescentTickCount()
        {
            if (delta == 1)
            {
                delta = 2;
            }
            else if (delta == 2)
            {
                delta = 5;
            }
            else if (delta == 5)
            {
                delta = 1;
                beta++;
            }
        }

        public void IncreaseTickCount()
        {
            if (delta == 1)
            {
                delta = 5;
                beta--;
            }
            else if (delta == 2)
            {
                delta = 1;
            }
            else if (delta == 5)
            {
                delta = 2;
            }
        }

        public void Initialize(Range<double> value)
        {
            delta = 1;
            beta = (int)Math.Round(Math.Log10(value.Max - value.Min)) - 1;
        }

        public double[] CreateTicks(Range<double> value, bool only_inside = true)
        {
            double start = value.Min;
            double finish = value.Max;
            double d = finish - start;

            if (d == 0)
                return new double[] { start, finish };

            double temp = CalculateInterval();
            double min = Math.Floor(start / temp);
            double max = Math.Floor(finish / temp);
            int count = (int)(max - min + 1);
            List<double> res = new List<double>();
            double x0 = min * temp;
            for (int i = 0; i < count + 1; i++)
            {
                double v = RoundHelper.Round(x0 + i * temp, beta);
                if (only_inside)
                {
                    if (v >= start && v <= finish)
                    {
                        res.Add(v);
                    }
                }
                else
                {
                    res.Add(v);
                }
            }
            return res.ToArray();
        }
    }
}
