using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface ILinearAxisVisualModel
    {
        bool ShowMajorTick { get; }

        bool ShowMinorTick { get; }

        double MajorTickLength { get; }

        double MinorTickLength { get; }

        double? Minimum { get; }

        double? Maximum { get; }
    }

    public class DefaultLinearAxisVisualModel : ILinearAxisVisualModel
    {
        public bool ShowMajorTick { get; set; }

        public bool ShowMinorTick { get; set; }

        public double MajorTickLength { get; set; }

        public double MinorTickLength { get; set; }

        public double? Minimum { get; set; }

        public double? Maximum { get; set; }
    }
}
