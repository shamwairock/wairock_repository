using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class LinearRealTimeXAxis : AdaptiveRangeAxis, IGridLineSetter, IGridLineProvider
    {
        public LinearRealTimeXAxis(IAxisDataType dataConverter)
           : base(dataConverter)
        {

        }

        private object displayLength = 100.0;
        public object DisplayLength
        {
            get { return displayLength; }
            set { displayLength = value; UpdateScale(); }
        }

        public object ToDataValue(double value)
        {
            return this.DataConverter.ToDataValue(value);
        }

        public double ToAxisDouble(object value)
        {
            return this.DataConverter.ToAxisDouble(value);
        }

        protected bool InRange(double value, double rangeLeft, double rangeRight)
        {
            return value >= rangeLeft && value <= rangeRight;
        }

        public override object GetPointValue(IDataPoint point)
        {
            return point.X;
        }

        protected override bool HasValidRange()
        {
            return false;
        }

        protected override void SetScale(object startValue, object stopValue, bool autoRange)
        {
        }

        public override double GetStartValuePixelsPos()
        {
            return TopLeftPosPixels;
        }

        public override double GetEndValuePixelsPos()
        {
            return BottomRightPosPixels;
        }

        private AxisPosition tilePosition = AxisPosition.AtBottom;
        public override AxisPosition TilePosition
        {
            get
            {
                return tilePosition;
            }
            set
            {
                tilePosition = value;
                RaiseNotifyPropertyChanged("TilePosition");
            }
        }

        private AxisPosition axisPosition = AxisPosition.AtRight;
        public override AxisPosition AxisPosition
        {
            get
            {
                return axisPosition;
            }
            set
            {
                axisPosition = value;
                RaiseNotifyPropertyChanged("AxisPosition");
            }
        }

        private GridLinePosition[] gridlines = null;
        public void SetGridLines(GridLinePosition[] lines)
        {
            gridlines = lines;
        }

        public GridLinePosition[] GetGridLines()
        {
            return gridlines;
        }
    }

    public abstract class DateTimeRealTimeXAxis : AdaptiveRangeAxis
    {
        public DateTimeRealTimeXAxis(IAxisDataType dataConverter)
           : base(dataConverter)
        {

        }

        private object displayLength = new TimeSpan(1, 0, 0, 0);
        public object DisplayLength
        {
            get { return displayLength; }
            set { displayLength = value; UpdateScale(); }
        }

        public object ToDataValue(double value)
        {
            return this.DataConverter.ToDataValue(value);
        }

        public double ToAxisDouble(object value)
        {
            return this.DataConverter.ToAxisDouble(value);
        }

        protected bool InRange(double value, double rangeLeft, double rangeRight)
        {
            return value >= rangeLeft && value <= rangeRight;
        }

        public override object GetPointValue(IDataPoint point)
        {
            return point.X;
        }

        protected override bool HasValidRange()
        {
            return false;
        }

        protected override void SetScale(object startValue, object stopValue, bool autoRange)
        {
        }

        public override double GetStartValuePixelsPos()
        {
            return TopLeftPosPixels;
        }

        public override double GetEndValuePixelsPos()
        {
            return BottomRightPosPixels;
        }

        private AxisPosition tilePosition = AxisPosition.AtBottom;
        public override AxisPosition TilePosition
        {
            get
            {
                return tilePosition;
            }
            set
            {
                tilePosition = value;
                RaiseNotifyPropertyChanged("TilePosition");
            }
        }

        private AxisPosition axisPosition = AxisPosition.AtRight;
        public override AxisPosition AxisPosition
        {
            get
            {
                return axisPosition;
            }
            set
            {
                axisPosition = value;
                RaiseNotifyPropertyChanged("AxisPosition");
            }
        }
    }
}
