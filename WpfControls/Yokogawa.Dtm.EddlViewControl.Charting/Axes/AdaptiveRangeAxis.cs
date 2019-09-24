using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class AdaptiveRangeAxis : Axis
    {
        protected bool isFixRange = false;

        protected object calcRangeStartValue = null;
        protected object calcRangeStopValue = null;

        protected object fixedRangeStartValue = null;
        protected object fixedRangeStopValue = null;

        private bool goToAutoScale = true;
        public bool GoToAutoScale
        {
            get { return goToAutoScale; }
            protected set { goToAutoScale = value; RaiseNotifyPropertyChanged("GoToAutoScale"); }
        }

        private bool autoScaleSetted = false;

        public AdaptiveRangeAxis(IAxisDataType dataConverter)
        {
            this.DataConverter = dataConverter;
        }

        public override void CalculateRange(IList<DataSeries> dataSeries)
        {
            if (autoScaleSetted)
            {
                if (this.goToAutoScale)
                {
                    CalculateAutoRange(dataSeries);
                }
                else
                {
                    if (HasValidRange())
                    {
                        CalculateFixedRange(dataSeries);
                    }
                }
            }
            else
            {
                if (HasValidRange())
                {
                    CalculateFixedRange(dataSeries);
                }
                else
                {
                    CalculateAutoRange(dataSeries);
                }
            }
            UpdateScale();
        }

        public override void UpdateScale()
        {
            if (this.isFixRange)
            {
                SetScale(this.fixedRangeStartValue, this.fixedRangeStopValue, false);
            }
            else
            {
                SetScale(this.calcRangeStartValue, this.calcRangeStopValue, true);
            } 
        }

        public virtual void FixRangeTo(object startValue, object endValue)
        {
            isFixRange = true;
            fixedRangeStartValue = startValue;
            fixedRangeStopValue = endValue;
            UpdateScale();
        }

        public virtual void ReleaseFixRange()
        {
            isFixRange = false;
            UpdateScale();
        }

        public virtual void ChanageAutoScale(bool autoScale)
        {
            autoScaleSetted = true;
            this.GoToAutoScale = autoScale;
            UpdateScale();
        }

        public virtual void ReleaseAutoScale()
        {
            autoScaleSetted = false;
            this.GoToAutoScale = HasValidRange();
            UpdateScale();
        }

        protected virtual void CalculateAutoRange(IList<DataSeries> dataSeries)
        {
            if (this.DataConverter == null || dataSeries == null)
            {
                return;
            }

            object s = null;
            object e = null;

            foreach (var series in dataSeries)
            {
                if (series != null && series.Points != null)
                {
                    Range<IDataPoint> range;
                    bool hasRange = false;
                    if (Orientation == AxisOrientation.Horizontal)
                    {
                        hasRange = series.Points.GetRangeX(out range);
                    }
                    else
                    {
                        hasRange = series.Points.GetRangeY(out range);
                    }
                    if (hasRange)
                    {
                        try
                        {
                            if (this.DataConverter.ValidData(GetPointValue(range.Min)))
                            {
                                if (s == null || this.DataConverter.Compare(s, GetPointValue(range.Min)) > 0)
                                {
                                    s = GetPointValue(range.Min);
                                }

                                if (e == null || this.DataConverter.Compare(e, GetPointValue(range.Min)) < 0)
                                {
                                    e = GetPointValue(range.Min);
                                }
                            }

                            if (this.DataConverter.ValidData(GetPointValue(range.Max)))
                            {
                                if (s == null || this.DataConverter.Compare(s, GetPointValue(range.Max)) > 0)
                                {
                                    s = GetPointValue(range.Max);
                                }

                                if (e == null || this.DataConverter.Compare(e, GetPointValue(range.Max)) < 0)
                                {
                                    e = GetPointValue(range.Max);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            this.calcRangeStartValue = s;
            this.calcRangeStopValue = e;
        }

        protected virtual void CalculateFixedRange(IList<DataSeries> dataSeries)
        {
            if (this.calcRangeStartValue == null || this.calcRangeStopValue == null)
            {
                CalculateAutoRange(dataSeries);
            }
        }

        public abstract object GetPointValue(IDataPoint point);

        protected abstract bool HasValidRange();

        public bool IsAutoScale
        {
            get
            {
                if (autoScaleSetted)
                {
                    return this.GoToAutoScale;
                }
                else
                {
                    return !HasValidRange();
                }
            }
        }
    }
}
