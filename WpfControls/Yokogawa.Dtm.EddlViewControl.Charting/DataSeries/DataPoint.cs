using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    [Flags]
    public enum DataQuality
    {
        GoodPoint = 0x00,
        WarningPoint = 0x01,
        BadPoint = 0x02,
        ExceptionPoint = 0x04,
        StartPoint = 0x10,
        EndPoint = 0x20,
    }

    public interface IDataPoint
    {
        DataQuality Quality { get; }
        bool Emphasis { get; }
        object X { get; }
        object Y { get; }
    }

    public interface IDataRange
    {
        object Start { get; }
        object End { get; }
    }

    public struct DataPoint<TX, TY> : IDataPoint where TX : IComparable where TY : IComparable
    {
        private TX x;
        private TY y;
        private bool emphasis;
        private DataQuality quality;

        public object X { get { return x; } }
        public object Y { get { return y; } }

        public bool Emphasis { get { return emphasis; } set { emphasis = value; } }
        public DataQuality Quality { get { return quality; } set { quality = value; } }

        public DataPoint(TX x, TY y, bool emphasis, DataQuality quality = DataQuality.GoodPoint)
        {
            this.emphasis = emphasis;
            this.quality = quality;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
