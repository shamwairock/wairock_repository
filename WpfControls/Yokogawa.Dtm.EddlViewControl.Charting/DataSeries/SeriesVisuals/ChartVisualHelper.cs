using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartVisualHelper
    {
        public static Type ConvertVisualType(ChartVisualType visualType)
        {
            Type type = null;
            switch (visualType)
            {
                case ChartVisualType.POLYLINE:
                    type = typeof(ChartPolylineCurve);
                    break;
                case ChartVisualType.BEZIER:
                    type = typeof(ChartBezierCurve);
                    break;
                case ChartVisualType.SCATTER:
                    type = typeof(ChartScatteredPoints);
                    break;
                case ChartVisualType.HORIZONTALLINE:
                    type = typeof(ChartHorizontalLine);
                    break;
                case ChartVisualType.VERTICALLINE:
                    type = typeof(ChartVerticalLine);
                    break;
                case ChartVisualType.HORIZONTALBAR:
                    type = typeof(ChartHorizontalBar);
                    break;
                case ChartVisualType.VERTICALBAR:
                    type = typeof(ChartVerticalBar);
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}
