using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public struct GridLinePosition
    {
        public GridLinePosition(double pos, bool _isLong)
        {
            position = pos;
            isLong = _isLong;
        }

        private double position;
        public double Position
        {
            get { return position; }
            set { position = value; }
        }
        private bool isLong;
        public bool IsLong
        {
            get { return isLong; }
            set { isLong = value; }
        }
        //public double Position { get; private set; }
        //public bool IsLong { get; private set; }
    }

    public interface IGridLineProvider
    {
        GridLinePosition[] GetGridLines();
    }

    public interface IGridLineSetter
    {
        void SetGridLines(GridLinePosition[] lines);
    }
}
