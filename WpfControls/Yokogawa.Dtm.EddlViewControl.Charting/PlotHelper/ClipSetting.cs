using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
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
