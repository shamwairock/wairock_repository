using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class YAxis : AdaptiveRangeAxis
    {
        public YAxis(IAxisDataType dataConverter)
            :base(dataConverter)
        {
        }

        protected override void CalculateAutoRange(IList<DataSeries> dataSeries)
        {
            if (this.AxisDataManager == null || dataSeries == null)
            {
                return;
            }
            object s = null;
            object e = null;

            foreach (var series in dataSeries)
            {
                if (series != null && series.Points != null)
                {
                    foreach (var p in series.Points)
                    {
                        try
                        {
                            if (!this.AxisDataManager.ValidData(p.Y))
                            {
                                continue;
                            }

                            if (s == null || this.AxisDataManager.Compare(s, p.Y) > 0)
                            {
                                s = p.Y;
                            }

                            if (e == null || this.AxisDataManager.Compare(e, p.Y) < 0)
                            {
                                e = p.Y;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            this.calcRangeStartValue = s;
            this.calcRangeStopValue = e;
        }

        protected override void CalculateFixedRange(IList<DataSeries> dataSeries)
        {
            if (this.calcRangeStartValue == null || this.calcRangeStopValue == null)
            {
                CalculateAutoRange(dataSeries);
            }
        }

        protected override double GetStartValuePixelsPos(object startValue)
        {
            return this.BottomRightPosPixels;
        }

        protected override double GetEndValuePixelsPos(object endValue)
        {
            return this.TopLeftPosPixels;
        }
    }
}
