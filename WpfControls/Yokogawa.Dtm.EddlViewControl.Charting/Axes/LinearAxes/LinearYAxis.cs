using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LinearYAxis : LinearAxis
    {
        public override object AxisTypeName
        {
            get
            {
                return this.GetType();
            }
        }

        public LinearYAxis(IAxisDataType dataConverter)
            : base(dataConverter)
        {
            UpdateScale();
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            if(src != null && src.Count() > 0)
            {
                var ret = new List<Plot>();
                int count = src.Count();
                var l = new List<IDataPoint>();
                for(int i=0;i<count/2; i++)
                {
                    l.Add(src.ToArray()[i]);
                }
                ret.Add(new Plot() { Points = l });
                l = new List<IDataPoint>();
                for (int j = count /2 + 5; j < count; j++)
                {
                    l.Add(src.ToArray()[j]);
                }
                ret.Add(new Plot() { Points = l });
                return ret;
            }
            return base.ToPlot(src);
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
