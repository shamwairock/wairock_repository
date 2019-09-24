using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicXAxis : LogarithmicAxis
    {
        public override object AxisTypeName
        {
            get
            {
                return this.GetType();
            }
        }

        public LogarithmicXAxis(IAxisDataType dataConverter, bool exponentTickLabel=true)
            : base(dataConverter, exponentTickLabel)
        {
            UpdateScale();
        }

        public override object GetPointValue(IDataPoint point)
        {
            return point.X;
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

        private AxisPosition axisPosition = AxisPosition.AtTop;
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
            get { return AxisOrientation.Horizontal; }
        }
    }
}
