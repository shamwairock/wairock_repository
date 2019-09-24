using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public struct LogarithmicValue
    {
        public LogarithmicValue(double _a, int _n)
        {
            a = _a;
            n = _n;
            val = a * Math.Pow(10.0, n);
        }

        private double a;
        public double A
        {
            get { return a; }
            set { a = value; }
        }
        private int n;
        public int N
        {
            get { return n; }
            set { n = value; }
        }
        private double val;
        public double Value
        {
            get { return val; }
            set { val = value; }
        }
        //public double A { get; private set; }
        //public int N { get; private set; }
        //public double Value { get; private set; }
    }
}
