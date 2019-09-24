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
        public override double Minimum
        {
            get { return start; }
        }

        double stop = 1000.0;
        public override double Maximum
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
                    double start = Convert.ToDouble(Minimum);
                    if (double.IsNaN(start) || double.IsInfinity(start))
                        return false;
                    double stop = Convert.ToDouble(Maximum);
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

        public override double MinDefaultRange
        {
            get { return 1.0; }
        }

        public override double MaxDefaultRange
        {
            get { return 1000.0; }
        }

        public override double GetPositionByValue(double value)
        {
            return this.StartPixels + Math.Log10(value / this.Minimum) * this.Scale;
        }

        public override double GetValueByPoistion(double pixels)
        {
            pixels = pixels - this.startPixels;
            return this.Minimum * Math.Pow(10, pixels / this.Scale);
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
                !double.IsNaN(startPixels) && !double.IsInfinity(startPixels))
            {
                bool minMaxDirection = start < stop;

                double ss = start < stop ? start : stop;

                double ee = start > stop ? start : stop;

                if (ee > 0 && ss <= 0)
                {
                    ss = 1;
                    //ss = CalculateLogarithmicRangeValue(ee, false);
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
            this.scale = extent / Math.Log10(this.Maximum / this.Minimum);
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
            if (double.IsNaN(ss) || double.IsNaN(ee) || double.IsPositiveInfinity(ss) || double.IsPositiveInfinity(ee))
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
    }
}
