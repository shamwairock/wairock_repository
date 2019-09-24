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
        public override double Minimum
        {
            get { return start; }
        }

        double stop = 100.0;
        public override double Maximum
        {
            get { return stop; }
        }

        private double startPosistion = 0;
        public double StartPosistion
        {
            get { return startPosistion; }
        }

        private double stopPosition = 0;
        public double StopPosistion
        {
            get { return stopPosition; }
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

        public override double MinDefaultRange
        {
            get { return 0.0; }
        }

        public override double MaxDefaultRange
        {
            get { return 100.0; }
        }

        public override void SetScale(double start, double stop, double startPixels, double stopPixels, bool autoRange)
        {
            AutoArrage(start, stop, startPixels, stopPixels, autoRange);
        }

        private void AutoArrage(double startValue, double stopValue, double startPixels, double stopPixels, bool autoRange)
        {
            if (startValue == stopValue || startPixels == stopPixels)
            {
                return;
            }

            bool minMaxDirection = startValue < stopValue;

            double ss = startValue < stopValue ? startValue : stopValue;

            double ee = startValue > stopValue ? startValue : stopValue;

            //double range = NiceNum(ee - ss, false);
            //double d = NiceNum(range / (tickCount - 1), true);
            //double graphmin = Math.Floor(ss / d) * d;
            //double graphmax = Math.Ceiling(ee / d) * d;

            double graphmin = ss;
            double graphmax = ee;
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
            this.startPosistion = startPixels;
            this.stopPosition = stopPixels;
            var extent = this.stopPosition - this.startPosistion;
            this.scale = extent / Math.Abs(this.Maximum - this.Minimum);
        }

        public override double GetPositionByValue(double value)
        {
            return (value - this.Minimum) * this.Scale + this.StartPosistion;
        }

        public override double GetValueByPoistion(double pixels)
        {
            pixels = pixels - this.StartPosistion;
            return this.Scale == 0 ? 0 : (pixels / this.Scale + Minimum);
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

    }
}
