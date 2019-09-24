using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IDataSeries 
    {
        Axis XAxis { get; set; }

        Axis YAxis { get; set; }
    }

    public class DataSeries : DependencyObject, IDataSeries, INotifyPropertyChanged, IPointMarker
    {
        private long pointsCapacity = 3600;

        public long PointsCapacity
        {
            get { return pointsCapacity; }
            set 
            {
                if (value > 0)
                {
                    pointsCapacity = value;
                    NotifyPropertyChanged("PointsCapacity");
                }
            }
        }

        string seriesName;
        public string SeriesName
        {
            get { return seriesName; }
            set
            {
                if (seriesName != value)
                {
                    seriesName = value;
                    NotifyPropertyChanged("SeriesName");
                }
            }
        }

        IDataPoints points;
        public IDataPoints Points
        {
            get { return points; }
            set
            {
                if (points != value)
                {
                    if (points != null)
                    {
                        INotifyCollectionChanged iNotifyCollectionChanged = points as INotifyCollectionChanged;
                        if (iNotifyCollectionChanged != null)
                        {
                            iNotifyCollectionChanged.CollectionChanged -= CollectionChanged;
                        }
                    }

                    points = value;
                    if (points != null)
                    {
                        INotifyCollectionChanged iNotifyCollectionChanged = points as INotifyCollectionChanged;
                        if (iNotifyCollectionChanged != null)
                        {
                            iNotifyCollectionChanged.CollectionChanged += CollectionChanged;
                        }
                    }
                    NotifyPropertyChanged("Points");
                }
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (points.Count > PointsCapacity)
            {
                points.RemoveAt(0);
            }
        }

        Pen pen = new Pen(Brushes.Red, 1);
        public Pen Pen
        {
            get { return pen; }
            set
            {
                if (pen != value)
                {
                    pen = value;
                    NotifyPropertyChanged("HorizontalPen");
                }
            }
        }

        Drawing pointMarker;
        public Drawing PointMarker
        {
            get { return pointMarker; }
            set
            {
                if (pointMarker != value)
                {
                    pointMarker = value;
                    NotifyPropertyChanged("PointMarker");
                }
            }
        }

        private Drawing emphasisPointMarker;
        public Drawing EmphasisPointMarker
        {
            get { return emphasisPointMarker; }
            set 
            {
                if (emphasisPointMarker != value)
                {
                    emphasisPointMarker = value;
                    NotifyPropertyChanged("EmphasisPointMarker");
                }
            }
        }

        bool pointMarkerVisible = true;
        public bool PointMarkerVisible
        {
            get { return pointMarkerVisible; }
            set
            {
                if (pointMarkerVisible != value)
                {
                    pointMarkerVisible = value;
                    NotifyPropertyChanged("PointMarkerVisible");
                }
            }
        }

        #region VisualCue
        public virtual object VisualCue
        {
            get
            {
                return ChartVisualHelper.ConvertVisualType(this.VisualType);
            }
        }
        #endregion VisualCue

        #region ChartVisualType
        public ChartVisualType VisualType
        {
            get { return (ChartVisualType)GetValue(VisualTypeProperty); }
            set { SetValue(VisualTypeProperty, value); }
        }

        public static readonly DependencyProperty VisualTypeProperty =
            DependencyProperty.Register("VisualType", typeof(ChartVisualType), typeof(DataSeries), new FrameworkPropertyMetadata(ChartVisualType.POLYLINE, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnVisualTypeChanged)));

        private static void OnVisualTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            if (dataSeries != null)
            {
                dataSeries.NotifyPropertyChanged("VisualCue");
            }
        }
        #endregion ChartVisualType

        #region PlotFlowAxisOption

        public static readonly DependencyProperty PlotFlowAxisOptionProperty
          = DependencyProperty.Register("PlotFlowAxisOption", typeof(PlotFlowAxisOptions), typeof(DataSeries),
              new FrameworkPropertyMetadata(PlotFlowAxisOptions.XAxis,
                  FrameworkPropertyMetadataOptions.AffectsMeasure |
                  FrameworkPropertyMetadataOptions.AffectsRender,
                  new PropertyChangedCallback(OnPlotFlowAxisOptionPropertyChanged)));

        public PlotFlowAxisOptions PlotFlowAxisOption
        {
            get { return (PlotFlowAxisOptions)GetValue(PlotFlowAxisOptionProperty); }
            set { SetValue(PlotFlowAxisOptionProperty, value); }
        }

        private static void OnPlotFlowAxisOptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as DataSeries;
            if (series != null)
            {
                series.NotifyPropertyChanged("VisualCue");
            }
        }
        #endregion

        #region X/Y Axis

        public Axis XAxis { get; set; }

        public Axis YAxis { get; set; }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged Members 
    
        #region IsVisible

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(DataSeries),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnIsVisibleChanged),
                    new CoerceValueCallback(OnIsVisibleCoerceValue)));

        private static object OnIsVisibleCoerceValue(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            if (dataSeries != null)
            {
                dataSeries.NotifyPropertyChanged("IsVisible");
                dataSeries.NotifyPropertyChanged("VisualCue");
            }
        }

        #endregion

        #region Bar Width
        public double BarWidth
        {
            get { return (double)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }

        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register("BarWidth", typeof(double), typeof(DataSeries), 
                new FrameworkPropertyMetadata((double)10.0, 
                    FrameworkPropertyMetadataOptions.AffectsRender | 
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnBarWidthChanged),
                    new  CoerceValueCallback(OnBarWidthCoerceValue)));

        private static object OnBarWidthCoerceValue(DependencyObject d, object baseValue)
        {
            if(baseValue == null)
            {
                return (double)10.0;
            }
            if(double.IsNaN((double)baseValue) || double.IsInfinity((double)baseValue) || (double)baseValue < 0)
            {
                return (double)10.0;
            }
            return baseValue;
        }

        private static void OnBarWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataSeries dataSeries = d as DataSeries;
            if (dataSeries != null)
            {
                dataSeries.NotifyPropertyChanged("VisualCue");
                dataSeries.NotifyPropertyChanged("BarWidth");
            }
        }
        #endregion Bar Width
    }
}
