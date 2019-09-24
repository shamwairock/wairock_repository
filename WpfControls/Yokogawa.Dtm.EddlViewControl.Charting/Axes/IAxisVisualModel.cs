using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IAxisVisualModel
    {
        FontFamily FontFamily { get;  }

        double FontSize { get;  }

        FontWeight FontWeight { get;  }

        FontStyle FontStyle { get;  }

        FontStretch FontStretch { get;  }

        Brush LabelBrush { get;  }

        double TitleFontSize { get;  }

        Brush TitleBrush { get;  }

        AxisOrientation Orientation { get;  }

        bool ShowTitle { get;  }

        string Title { get;  }

        AxisContentLayout ContentLayout { get;  }

        Pen TickPen { get;  }

        Pen TickBaseLinePen { get; }

        double TitleMargin { get;  }

        double LabelMargin { get; }

        double PlotLength { get; }

        double PlotStartPos { get;  }

        double PlotStopPos { get;  }

        IAxisDataType DataConverter { get; }

        Func<object, string> CustomFormatter { get; }

        Func<string, string> TitleCustomFormatter { get; }

        double? MinMajorTickInterval { get; }

        double? MaxMajorTickInterval { get; }
    }

    public class DefaultAxisVisualModel : IAxisVisualModel
    {
        public FontFamily FontFamily { get; set; }

        public double FontSize { get; set; }

        public FontWeight FontWeight { get; set; }

        public FontStyle FontStyle { get; set; }

        public FontStretch FontStretch { get; set; }

        public Brush LabelBrush { get; set; }

        public double TitleFontSize { get; set; }

        public Brush TitleBrush { get; set; }

        public AxisOrientation Orientation { get; set; }

        public bool ShowTitle { get; set; }

        public string Title { get; set; }

        public AxisContentLayout ContentLayout { get; set; }

        public Pen TickPen { get; set; }

        public Pen TickBaseLinePen { get; set; }

        public double TitleMargin { get; set; }

        public double LabelMargin { get; set; }

        public double PlotLength { get; set; }

        public double PlotStartPos { get; set; }

        public double PlotStopPos { get; set; }

        public IAxisDataType DataConverter { get; set; }

        public Func<object, string> CustomFormatter { get; set; }

        public Func<string, string> TitleCustomFormatter { get; set; }

        public double? MinMajorTickInterval { get; set; }

        public double? MaxMajorTickInterval { get; set; }
    }
}
