using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface ISweepLinearAxisVisualModel
    {
        bool ShowMajorTick { get; }

        bool ShowMinorTick { get; }

        double MajorTickLength { get; }

        double MinorTickLength { get; }

        double? PreMinimum { get; }

        double? PreMaximum { get; }

        double? PostMinimum { get; }

        double? PostMaximum { get; }

        bool PostPartValid { get; }
    }

    public class DefaultSweepLinearAxisVisualModel : ISweepLinearAxisVisualModel
    {
        public bool ShowMajorTick { get; set; }

        public bool ShowMinorTick { get; set; }

        public double MajorTickLength { get; set; }

        public double MinorTickLength { get; set; }

        public double? PreMinimum { get; set; }

        public double? PreMaximum { get; set; }

        public double? PostMinimum { get; set; }

        public double? PostMaximum { get; set; }

        public bool PostPartValid { get; set; }
    }
}
