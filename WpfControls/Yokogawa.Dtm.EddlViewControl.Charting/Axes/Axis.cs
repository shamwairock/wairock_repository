using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class Axis : DependencyObject, IPlotConverter, INotifyPropertyChanged
    {
        #region Tick / Tick Label
        private readonly Pen defaultTickPen = new Pen(Brushes.Black, 1);
        private Pen tickPen = null;
        public virtual Pen TickPen
        {
            get { return tickPen != null ? tickPen : defaultTickPen; }
            set { tickPen = value; RaiseNotifyPropertyChanged("TickPen"); }
        }

        private Pen tickBaseLinePen = null;
        public virtual Pen TickBaseLinePen
        {
            get { return tickBaseLinePen != null ? tickBaseLinePen : defaultTickPen; }
            set { tickBaseLinePen = value; RaiseNotifyPropertyChanged("TickBaseLinePen"); }
        }

        private readonly Pen defaultTickLabelPen = new Pen(Brushes.Black, 1);
        private Pen tickLabelPen = null;
        public virtual Pen TickLabelPen
        {
            get { return tickLabelPen != null ? tickLabelPen : defaultTickLabelPen; }
            set { tickLabelPen = value; RaiseNotifyPropertyChanged("TickLabelPen"); }
        }

        private double tickLabelFontSize = 12;
        public double TickLabelFontSize
        {
            get { return tickLabelFontSize; }
            set { tickLabelFontSize = value; RaiseNotifyPropertyChanged("TickLabelFontSize"); }
        }

        private bool showTickLabel = true;
        public virtual bool ShowTickLabel
        {
            get { return showTickLabel; }
            set { showTickLabel = value; RaiseNotifyPropertyChanged("ShowTickLabel"); }
        }

        private bool showMayorTick = true;
        public bool ShowMajorTick
        {
            get { return showMayorTick; }
            set { showMayorTick = value; RaiseNotifyPropertyChanged("ShowMajorTick"); }
        }

        private bool showMinorTick = true;
        public bool ShowMinorTick
        {
            get { return showMinorTick; }
            set { showMinorTick = value; RaiseNotifyPropertyChanged("ShowMinorTick"); }
        }

        private double mayorTickLength = 10.0;
        public virtual double MajorTickLength
        {
            get { return mayorTickLength; }
            set
            {
                mayorTickLength = value;
                RaiseNotifyPropertyChanged("MajorTickLength");
            }
        }

        private double minorTickLength = 5.0;
        public virtual double MinorTickLength
        {
            get { return minorTickLength; }
            set
            {
                minorTickLength = value;
                RaiseNotifyPropertyChanged("MinorTickLength");
            }
        }

        #endregion

        #region MajorTickInterval Min/Max

        private double? minMajorTickInterval = null;
        public double? MinMajorTickInterval
        {
            get { return minMajorTickInterval; }
            set
            {
                minMajorTickInterval = value;
                RaiseNotifyPropertyChanged("MinMajorTickInterval");
            }
        }

        private double? maxMajorTickInterval = null;
        public double? MaxMajorTickInterval
        {
            get { return maxMajorTickInterval; }
            set
            {
                maxMajorTickInterval = value;
                RaiseNotifyPropertyChanged("MaxMajorTickInterval");
            }
        }

        #endregion

        #region Axis Title / Help
        private string title = "";
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaiseNotifyPropertyChanged("Title");
            }
        }

        private readonly Pen defaultTitlePen = new Pen(Brushes.Black, 1);
        private Pen titlePen = null;
        public virtual Pen TitlePen
        {
            get { return titlePen != null ? titlePen : defaultTitlePen; }
            set
            {
                titlePen = value;
                RaiseNotifyPropertyChanged("Title");
                RaiseNotifyPropertyChanged("TitlePen");
            }
        }

        private double titleFontSize = 16;
        public double TitleFontSize
        {
            get { return titleFontSize; }
            set { titleFontSize = value; RaiseNotifyPropertyChanged("TitleFontSize"); }
        }

        private bool showTitle = true;
        public virtual bool ShowTitle
        {
            get { return showTitle; }
            set
            {
                showTitle = value;
                RaiseNotifyPropertyChanged("ShowTitle");
            }
        }

        private string help = string.Empty;
        public virtual string Help
        {
            get { return help; }
            set { help = value; RaiseNotifyPropertyChanged("Help"); }
        }
        #endregion

        #region AxisGridLine

        private bool showGridLine = true;
        public bool ShowGridLine
        {
            get { return showGridLine; }
            set { showGridLine = value; RaiseNotifyPropertyChanged("ShowGridLine"); }
        }

        private Pen gridLinePen = null;
        public virtual Pen GridLinePen
        {
            get { return gridLinePen; }
            set { gridLinePen = value; RaiseNotifyPropertyChanged("GridLinePen"); }
        }

        #endregion

        private double extent = 100.0;
        public virtual double Extent
        {
            get { return extent; }
            set { extent = value; RaiseNotifyPropertyChanged("Extent"); }
        }

        private double topLeftPosScale = 0;
        public virtual double TopLeftPosScale
        {
            get { return topLeftPosScale; }
            set { topLeftPosScale = value; OnTopLeftPosScaleChanged(); }
        }

        private double bottomRightPosScale = 1;
        public virtual double BottomRightPosScale
        {
            get { return bottomRightPosScale; }
            set { bottomRightPosScale = value; OnBottomRightPosScaleChanged(); }
        }

        private void OnTopLeftPosScaleChanged()
        {
            this.topLeftPosPixels = this.Extent * this.topLeftPosScale;
            RaiseNotifyPropertyChanged("TopLeftPosScale");
            RaiseNotifyPropertyChanged("TopLeftPosPixels");
            RaiseNotifyPropertyChanged("InfoPosTrans");
        }

        private void OnBottomRightPosScaleChanged()
        {
            this.bottomRightPosPixels = this.Extent * this.bottomRightPosScale;
            RaiseNotifyPropertyChanged("BottomRightPosScale");
            RaiseNotifyPropertyChanged("BottomRightPosPixels");
            RaiseNotifyPropertyChanged("InfoPosTrans");
        }

        private double topLeftPosPixels = 0;
        public virtual double TopLeftPosPixels
        {
            get { return topLeftPosPixels; }
            set
            {
                topLeftPosPixels = value;
                RaiseNotifyPropertyChanged("TopLeftPosPixels");
                RaiseNotifyPropertyChanged("InfoPosTrans");
            }
        }

        private double bottomRightPosPixels = 100.0;
        public virtual double BottomRightPosPixels
        {
            get { return bottomRightPosPixels; }
            set
            {
                bottomRightPosPixels = value;
                RaiseNotifyPropertyChanged("BottomRightPosPixels");
                RaiseNotifyPropertyChanged("InfoPosTrans");
            }
        }

        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; RaiseNotifyPropertyChanged("IsVisible"); }
        }

        public abstract AxisPosition TilePosition { get; set; }

        public abstract AxisPosition AxisPosition { get; set; }

        public virtual Func<object, string> CustomFormatter { get; set; }

        public virtual Func<object, string> TitleCustomFormatter { get; set; }

        public virtual string LabelStringFormat { get; set; }

        protected abstract void SetScale(object startValue, object stopValue, bool autoRange);

        public abstract double ToPixels(object value);

        public virtual double ToPixels(object value, Scale scale)
        {
            return ToPixels(value);
        }

        public abstract object FromPixels(double value);

        public abstract void CalculateRange(IList<DataSeries> dataSeries);

        public abstract void UpdateScale();

        public abstract double GetStartValuePixelsPos();

        public abstract double GetEndValuePixelsPos();

        public abstract double StartPixelsPos { get; }

        public abstract double StopPixelsPos { get; }

        private List<Plot> defaultPlots = new List<Plot>();
        private Plot defaultPlot = new Plot();
        public virtual IEnumerable<Plot> ToPlot(IEnumerable<IDataPoint> src)
        {
            defaultPlots.Clear();
            defaultPlot.Points = src;
            defaultPlots.Add(defaultPlot);
            return defaultPlots;
        }

        public abstract object AxisTypeName { get; }

        private double labelMargin = 2.0;
        public double LabelMargin
        {
            get { return labelMargin; }
            set { labelMargin = value; }
        }

        public abstract AxisOrientation Orientation { get; }

        public IAxisDataType DataConverter { get; protected set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}