using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicYAxis : LogarithmicAxis
    {
        public override object AxisTypeName
        {
            get
            {
                return this.GetType();
            }
        }

        public LogarithmicYAxis(IAxisDataType dataConverter, bool exponentTickLabel = true)
            : base(dataConverter, exponentTickLabel)
        {
            UpdateScale();
        }

        public override object GetPointValue(IDataPoint point)
        {
            return point.Y;
        }

        public override double GetStartValuePixelsPos()
        {
            return BottomRightPosPixels;
        }

        public override double GetEndValuePixelsPos()
        {
            return TopLeftPosPixels;
        }


        private AxisPosition tilePosition = AxisPosition.AtLeft;
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

        public override AxisOrientation Orientation
        {
            get { return AxisOrientation.Vertical; }
        }
    }
}
