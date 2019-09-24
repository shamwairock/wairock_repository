using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class Scale
    {
        public abstract double Minimum { get; }

        public abstract double Maximum { get; }

        public abstract double MinDefaultRange { get; }

        public abstract double MaxDefaultRange { get; }

        public virtual bool IsConsistent
        {
            get { return (!double.IsNaN(Minimum) && (!double.IsNaN(Maximum)) && Minimum != Maximum); }
        }

        public abstract double GetPositionByValue(double value);

        public abstract double GetValueByPoistion(double value);

        public abstract void SetScale(double start, double stop, double starPixels, double stopPixels, bool autoRange);

        public abstract bool IsValidRangeData(double start, double stop);

        public abstract void GetValidRangeData(ref double ss, ref double ee, bool autoRange);
    }

}
