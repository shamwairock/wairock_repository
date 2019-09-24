using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public struct Range<T>
    {
        public Range(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        private readonly T min;
        public T Min
        {
            get { return min; }
        }

        private readonly T max;
        public T Max
        {
            get { return max; }
        }
    }
}
