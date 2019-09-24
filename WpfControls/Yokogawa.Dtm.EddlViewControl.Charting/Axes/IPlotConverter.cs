using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IPlotConverter
    {
        double ToPixels(object value, Scale scale);
    }
}
