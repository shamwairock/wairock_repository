using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class Plot
    {
        public Scale XScale { get; set; }
        public Scale YScale { get; set; }
        public IEnumerable<IDataPoint> Points { get; set; }
        public ClipSetting ClipSettings { get; set; }
    }

}
