using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class AutoRangeYAxis : YAxis
    {
        public AutoRangeYAxis(Scale scale, IAxisDataType dataConverter)
            :base(dataConverter)
        {
            this.AutoScale = true;
            this.Scale = scale;
            this.UpdateScale();
        }
        public override void ReleaseAutoScale()
        {
            this.AutoScale = true;
            UpdateScale();
        }
    }
}
