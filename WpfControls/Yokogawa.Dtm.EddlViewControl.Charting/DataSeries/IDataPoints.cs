using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IDataPoints : IList, IEnumerable, IEnumerable<IDataPoint>
    {
        void Initialize(IDataSeries series);

        IEnumerable<IDataPoint> GetPoints(IDataSeries series);

        bool GetRangeX(out Range<IDataPoint> range);

        bool GetRangeY(out Range<IDataPoint> range);
    }

    public class DataPoints<TX,TY> : DataList<DataPoint<TX, TY>>, IDataPoints where TX : IComparable where TY : IComparable
    {
        public IEnumerable<IDataPoint> GetPoints(IDataSeries series)
        {
            foreach(var p in this)
            {
                yield return p;
            }
        }

        public void Initialize(IDataSeries series)
        {
            xRangeFound = false;
            yRangeFound = false;

            IDataPoint xMin = null;
            IDataPoint xMax = null;
            IDataPoint yMin = null;
            IDataPoint yMax = null;

            bool xMinfound = false;
            bool xMaxfound = false;
            bool yMinfound = false;
            bool yMaxfound = false;

            foreach (var p in this)
            {
                if(p.X != null && p.X is TX)
                {
                    if(xMinfound == false)
                    {
                        xMin = p;
                        xMinfound = true;
                    }
                    else
                    {
                        if(((TX)xMin.X).CompareTo((TX)p.X) > 0)
                        {
                            xMin = p;
                        }
                    }
                    if(xMaxfound == false)
                    {
                        xMax = p;
                        xMaxfound = true;
                    }
                    else
                    {
                        if( ((TX)xMax.X).CompareTo((TX)p.X) < 0)
                        {
                            xMax = p;
                        }
                    }
                }

                if (p.Y != null && p.Y is TY)
                {
                    if (yMinfound == false)
                    {
                        yMin = p;
                        yMinfound = true;
                    }
                    else
                    {
                        if (((TY)yMin.Y).CompareTo((TY)p.Y) > 0)
                        {
                            yMin = p;
                        }
                    }
                    if (yMaxfound == false)
                    {
                        yMax = p;
                        yMaxfound = true;
                    }
                    else
                    {
                        if ( ((TY)yMax.Y).CompareTo((TY)p.Y) < 0)
                        {
                            yMax = p;
                        }
                    }
                }

            }

            if (xMinfound && xMaxfound)
            {
                xRange = new Range<IDataPoint>(xMin, xMax);
                xRangeFound = true;
            }
            if (yMinfound && yMaxfound)
            {
                yRange = new Range<IDataPoint>(yMin, yMax);
                yRangeFound = true;
            }
        }

        private Range<IDataPoint> xRange;
        private bool xRangeFound;

        public bool GetRangeX(out Range<IDataPoint> range)
        {
            range = xRange;
            return xRangeFound;
        }

        private Range<IDataPoint> yRange;
        private bool yRangeFound;
        public bool GetRangeY(out Range<IDataPoint> range)
        {
            range = yRange;
            return yRangeFound;
        }

        IEnumerator<IDataPoint> IEnumerable<IDataPoint>.GetEnumerator()
        { 
            foreach(var p in this)
            {
                yield return p;
            }
        }
    }
}
