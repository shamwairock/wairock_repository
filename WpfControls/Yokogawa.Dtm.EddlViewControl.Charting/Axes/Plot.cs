using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class Plot
    {
        public Scale XScale { get; set; }
        public Scale YScale { get; set; }
        public IEnumerable<IDataPoint> Points { get; set; }
        public ClipSetting ClipSettings { get; set; }
    }

    public static class PlotHelper
    {
        public static Rect GetClipRect(ClipSetting clipSetting, IPlotConverter xConverter, IPlotConverter yConverter, double xlong, double ylong)
        {
            double clipLeft = 0;
            double clipRight = xlong;
            double clipTop = 0;
            double clipBottom = ylong;
            if (clipSetting != null)
            {
                if (clipSetting.XClipRange != null)
                {
                    if (clipSetting.XClipRange.StartPoint != null)
                    {
                        clipLeft = xConverter.ToPixels(clipSetting.XClipRange.StartPoint.X, clipSetting.XClipRange.Scale);
                    }
                    if (clipSetting.XClipRange.EndPoint != null)
                    {
                        clipRight = xConverter.ToPixels(clipSetting.XClipRange.EndPoint.X, clipSetting.XClipRange.Scale);
                    }
                }

                if (clipSetting.YClipRange != null)
                {
                    if (clipSetting.YClipRange.StartPoint != null)
                    {
                        clipLeft = yConverter.ToPixels(clipSetting.YClipRange.StartPoint.Y, clipSetting.YClipRange.Scale);
                    }
                    if (clipSetting.YClipRange.EndPoint != null)
                    {
                        clipRight = yConverter.ToPixels(clipSetting.YClipRange.EndPoint.Y, clipSetting.YClipRange.Scale);
                    }
                }
            }
            return new Rect(new Point(clipLeft, clipTop), new Point(clipRight, clipBottom));
        }
    }

    public class ClipSetting
    {
        public ClipRangeSetting XClipRange { get; set; }
        public ClipRangeSetting YClipRange { get; set; }
    }
    public class ClipRangeSetting
    {
        public IDataPoint StartPoint { get; set; }
        public IDataPoint EndPoint { get; set; }
        public Scale Scale { get; set; }
    }
}
