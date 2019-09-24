using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface ISeriesRender
    {
        void Render(VisualContext context);
    }

    public class VisualContext
    {
        public IList<DataSeries> Sources { get; set; }
    }
}
