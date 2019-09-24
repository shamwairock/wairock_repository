using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class AutoRangeXAxis : XAxis
    {
        public AutoRangeXAxis(Scale scale, IAxisDataType dataConverter)
            :base(dataConverter)
        {
            this.AutoScale = true;
            this.Scale = scale;
            this.UpdateScale();
        }

        public override IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            return new List<Plot>() { new Plot() { Points = src } };
        }

        public override void ReleaseAutoScale()
        {
            this.AutoScale = true;
            UpdateScale();
        }
    }
}
