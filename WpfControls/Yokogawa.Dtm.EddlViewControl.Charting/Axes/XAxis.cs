using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class XAxis : AdaptiveRangeAxis
    {
        public XAxis(IAxisDataType dataConverter)
            :base(dataConverter)
        {
        }

        public abstract IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src);

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
                            if (!this.AxisDataManager.ValidData(p.X))
                            {
                                continue;
                            }

                            if (s == null || this.AxisDataManager.Compare(s, p.X) > 0)
                            {
                                s = p.X;
                            }

                            if (e == null || this.AxisDataManager.Compare(e, p.X) < 0)
                            {
                                e = p.X;
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
            return this.TopLeftPosPixels;
        }

        protected override double GetEndValuePixelsPos(object endValue)
        {
            return this.BottomRightPosPixels;
        }
    }
}
