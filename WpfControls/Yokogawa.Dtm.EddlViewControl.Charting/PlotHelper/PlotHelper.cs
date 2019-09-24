using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
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


}
