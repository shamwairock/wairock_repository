using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF.YOKOGAWA.Charting.Chart"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF.YOKOGAWA.Charting.Chart;assembly=WPF.YOKOGAWA.Charting.Chart"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Chart/>
    ///
    /// </summary>
    /// 
    [TemplatePart(Name = "PART_PlotterAreaBackground", Type = typeof(PlotterAreaBackground))]
    [TemplatePart(Name = "PART_AxesGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_PlotAreaBorder", Type = typeof(PlotterAreaBorder))] 
    [TemplatePart(Name = "PART_XAxesContainer", Type = typeof(HorizAxesContainer))]
    [TemplatePart(Name = "PART_YAxesContainer", Type = typeof(VerticalAxesContainer))]
    [TemplatePart(Name = "PART_PlotterArea", Type = typeof(PlotterArea))]
    public class Chart : Control, INotifyPropertyChanged
    {
        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

        ~Chart() { }

        private PlotterAreaBackground plotterAreaBackground = null;
        private Grid axesGrid = null;  
        private PlotterAreaBorder plotterAreaBorder = null;
        private HorizAxesContainer horizAxesContainer = null;
        private VerticalAxesContainer verticalAxesContainer = null;
        private PlotterArea plotterArea = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            plotterAreaBackground = this.Template.FindName("PART_PlotterAreaBackground", this) as PlotterAreaBackground;
            axesGrid = this.Template.FindName("PART_AxesGrid", this) as Grid;
            plotterAreaBorder = this.Template.FindName("PART_PlotAreaBorder", this) as PlotterAreaBorder;
            horizAxesContainer = this.Template.FindName("PART_XAxesContainer", this) as HorizAxesContainer;
            verticalAxesContainer = this.Template.FindName("PART_YAxesContainer", this) as VerticalAxesContainer;
            plotterArea = this.Template.FindName("PART_PlotterArea", this) as PlotterArea;

            plotterSize = new Size(1, 1);
            if (plotterArea != null)
            {
                plotterArea.PlotterAreaSizeChangedEvent += plotterArea_PlotterAreaSizeChangedEvent;
            }
        }

        public PlotterArea PlotterArea
        {
            get { return plotterArea; }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            UpdatePloterSize();
        }

        #region PlotterSize

        private Size plotterSize;

        public Size PlotterSize
        {
            get { return plotterSize; }
            set { plotterSize = value; }
        }   
  
        private void plotterArea_PlotterAreaSizeChangedEvent(object sender, PlotterAreaSizeChangedEventArgs e)
        {
            if (e.Size.Width > 0.0 && e.Size.Height > 0.0)
            {
                var plotMargin = PlotAreaMargin;
                this.PlotterSize = new Size(e.Size.Width, e.Size.Height);
                UpdatePloterSize();
            }
            Refresh();
        }
          
        public void UpdatePloterSize()
        {
            if (this.PlotterSize == null)
            {
                return;
            }
            double hExtent = this.PlotterSize.Width;
            double vExtent = this.PlotterSize.Height;
            UpdateXAxesDisplaySize(hExtent, vExtent);
            UpdateYAxesDisplaySize(hExtent, vExtent);
        }

        private void UpdateXAxesDisplaySize(double hExtent, double vExtent)
        {
            if (this.XAxes != null && this.XAxes.Count > 0)
            {
                foreach (var axis in this.XAxes)
                {
                    if (axis != null)
                    {
                        axis.Extent = hExtent;
                        axis.TopLeftPosPixels = axis.TopLeftPosScale * hExtent;
                        axis.BottomRightPosPixels = axis.BottomRightPosScale * hExtent;
                    }
                }
            }
        }

        private void UpdateYAxesDisplaySize(double hExtent, double vExtent)
        {
            if (this.YAxes != null && this.YAxes.Count > 0)
            {
                switch (this.YAxisZone)
                {
                    case AxisZoneTypes.AUTOZONE:
                        {
                            UpdateAutoZoneSize(vExtent, this.YAxes.ToArray());
                            break;
                        }
                    case AxisZoneTypes.SLIDEZONE:
                        {
                            UpdateSlideZoneSize(vExtent, this.YAxes.ToArray());
                            break;
                        }
                    case AxisZoneTypes.FULLZONE:
                        {
                            UpdateFullZoneSize(vExtent, this.YAxes.ToArray());
                            break;
                        }
                    default:
                        {
                            UpdateDefaultZoneSize(vExtent, this.YAxes.ToArray());
                            break;
                        }
                }
            }
        }

        private void UpdateDefaultZoneSize(double vExtent, Axis[] toShow)
        {
            if (toShow != null && toShow.Length > 0)
            {
                foreach (var axis in toShow)
                {
                    if (axis != null)
                    {
                        axis.Extent = vExtent;
                        axis.TopLeftPosPixels = vExtent * (axis.TopLeftPosScale);
                        axis.BottomRightPosPixels = vExtent * (axis.BottomRightPosScale);
                    }
                }
            }
        }

        private void UpdateFullZoneSize(double vExtent, Axis[] toShow)
        {
            if (toShow != null && toShow.Length > 0)
            {
                foreach (var axis in toShow)
                {
                    if (axis != null)
                    {
                        axis.Extent = vExtent;
                        axis.TopLeftPosPixels = 0;
                        axis.BottomRightPosPixels = vExtent;
                    }
                }
            }
        }

        private void UpdateSlideZoneSize(double vExtent, Axis[] toShow)
        {
            if (toShow != null && toShow.Length > 0)
            {
                double scaleStep = 0.05;
                double scaleStartCount = 0;
                double scaleEndCount = toShow.Length - 1;

                if (scaleEndCount * scaleStep > 0.5)
                {
                    scaleStep = 0.5 / scaleEndCount;
                }

                foreach (var axis in toShow)
                {
                    if (axis != null)
                    {
                        axis.Extent = vExtent;
                        axis.TopLeftPosPixels = vExtent * (scaleStep * scaleStartCount++);
                        axis.BottomRightPosPixels = vExtent * (1 - scaleStep * scaleEndCount--);
                    }
                }
            }
        }

        private static void UpdateAutoZoneSize(double vExtent, Axis[] toShow)
        {
            List<Axis> actualShowAxes = new List<Axis>();
            if (toShow != null && toShow.Length > 0)
            {
                foreach (var axis in toShow)
                {
                    if (axis != null && axis.IsVisible)
                    {
                        actualShowAxes.Add(axis);
                    }
                }
            }
            if (actualShowAxes != null && actualShowAxes.Count > 0)
            {
                var count = actualShowAxes.Count;
                var extentPer = (vExtent - (count - 1) * 2 * 10) / count;
                extentPer = extentPer > 0 ? extentPer : 0.1;
                double lastPos = 0;
                foreach (var axis in actualShowAxes)
                {
                    axis.Extent = vExtent;
                    axis.TopLeftPosPixels = lastPos;
                    axis.BottomRightPosPixels = lastPos + extentPer;
                    lastPos = axis.BottomRightPosPixels + 10 * 2;
                }
            }
        }

        #endregion

        #region Plot Area Margin

        public static readonly DependencyProperty PlotAreaMarginProperty
           = DependencyProperty.Register("PlotAreaMargin", typeof(Thickness), typeof(Chart),
               new FrameworkPropertyMetadata(new Thickness(20,20,20,20),
                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                   FrameworkPropertyMetadataOptions.AffectsArrange |
                   FrameworkPropertyMetadataOptions.AffectsRender,
                   new PropertyChangedCallback(OnPlotAreaMarginPropertyChanged),
                   new CoerceValueCallback(OnPlotAreaMarginCoerceValue)));

        public Thickness PlotAreaMargin
        {
            get { return (Thickness)GetValue(PlotAreaMarginProperty); }
            set { SetValue(PlotAreaMarginProperty, value); }
        }

        private static void OnPlotAreaMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static object OnPlotAreaMarginCoerceValue(DependencyObject d, object baseValue)
        {
            if (baseValue == null)
            {
                return new Thickness(20.0,20.0,20.0,20.0);
            }
            return baseValue;
        }

        public Thickness YAxisMargin
        {
            get
            {
                var plotMargin = PlotAreaMargin;
                return new Thickness(0, plotMargin.Top, 0, plotMargin.Bottom);
            }
        }

        public Thickness XAxisMargin
        {
            get
            {
                var plotMargin = PlotAreaMargin;
                return new Thickness(plotMargin.Left, 0, plotMargin.Right, 0);
            }
        }

        #endregion

        #region DataSeries
        public ObservableCollection<DataSeries> DataSeries
        {
            get { return (ObservableCollection<DataSeries>)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }

        public static readonly DependencyProperty DataSeriesProperty =
           DependencyProperty.Register("DataSeries", typeof(ObservableCollection<DataSeries>), typeof(Chart),
           new FrameworkPropertyMetadata(null, 
               FrameworkPropertyMetadataOptions.AffectsMeasure | 
               FrameworkPropertyMetadataOptions.AffectsArrange | 
               FrameworkPropertyMetadataOptions.AffectsRender,
               new PropertyChangedCallback(OnDataSeriesPropertyChanged)));

        private static void OnDataSeriesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            if (chart != null)
            {
                if (e.OldValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= chart.DataSeries_CollectionChanged;
                    }
                    var series = e.OldValue as IList<DataSeries>;
                    if (series != null)
                    {
                        foreach (var s in series)
                        {
                            s.PropertyChanged -= chart.DataSeries_PropertyChanged;
                        }
                    }
                }
                if (e.NewValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged += chart.DataSeries_CollectionChanged;
                    }
                    var series = e.NewValue as IList<DataSeries>;
                    if (series != null)
                    {
                        foreach (var s in series)
                        {
                            s.PropertyChanged += chart.DataSeries_PropertyChanged;
                        }
                    }
                }
                chart.RaiseNotifyPropertyChanged("DataSeries");
            }
        }

        private void DataSeries_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void DataSeries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var series = item as DataSeries;
                    if (series != null)
                    {
                        series.PropertyChanged -= DataSeries_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var series = item as DataSeries;
                    if (series != null)
                    {
                        series.PropertyChanged += DataSeries_PropertyChanged;
                    }
                }
            }
            RaiseNotifyPropertyChanged("DataSeries");
        }

        #endregion

        #region CurveSelectedEffect

        public static readonly DependencyProperty CurveSelectedEffectProperty
           = DependencyProperty.Register("CurveSelectedEffect", typeof(Effect), typeof(Chart), 
               new FrameworkPropertyMetadata(new DropShadowEffect() { BlurRadius = 2, Direction = 322, ShadowDepth = 5, Opacity = 0.5 }, 
                   FrameworkPropertyMetadataOptions.AffectsRender));

        public Effect CurveSelectedEffect
        {
            get { return (Effect)GetValue(CurveSelectedEffectProperty); }
            set { SetValue(CurveSelectedEffectProperty, value); }
        }

        #endregion

        #region Plot Background
        public static readonly DependencyProperty PlotAreaBackgroundProperty
            = DependencyProperty.Register("PlotAreaBackground", typeof(Brush), typeof(Chart), 
                new FrameworkPropertyMetadata(Brushes.Transparent, 
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush PlotAreaBackground
        {
            get { return (Brush)GetValue(PlotAreaBackgroundProperty); }
            set { SetValue(PlotAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PlotAreaBorderThicknessProperty
            = DependencyProperty.Register("PlotAreaBorderThickness", typeof(Thickness), typeof(Chart), 
                new FrameworkPropertyMetadata(new Thickness(), 
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public Thickness PlotAreaBorderThickness
        {
            get { return (Thickness)GetValue(PlotAreaBorderThicknessProperty); }
            set { SetValue(PlotAreaBorderThicknessProperty, value); }
        }        
        
        public static readonly DependencyProperty PlotAreaBorderBrushProperty
            = DependencyProperty.Register("PlotAreaBorderBrush", typeof(Brush), typeof(Chart), 
                new FrameworkPropertyMetadata(Brushes.Transparent, 
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush PlotAreaBorderBrush
        {
            get { return (Brush)GetValue(PlotAreaBorderBrushProperty); }
            set { SetValue(PlotAreaBorderBrushProperty, value); }
        }          
        #endregion

        #region AxisGrid
        public static readonly DependencyProperty HorizontalGridVisibilityProperty 
            = DependencyProperty.Register("HorizontalGridVisibility", typeof(GridVisibility), typeof(Chart), 
                new FrameworkPropertyMetadata(GridVisibility.ALLTICKS, 
                    FrameworkPropertyMetadataOptions.AffectsRender), GridVisibility_Validate);

        public GridVisibility HorizontalGridVisibility
        {
            get { return (GridVisibility)GetValue(HorizontalGridVisibilityProperty); }
            set { SetValue(HorizontalGridVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalGridVisibilityProperty 
            = DependencyProperty.Register("VerticalGridVisibility", typeof(GridVisibility), typeof(Chart), 
                new FrameworkPropertyMetadata(GridVisibility.ALLTICKS, FrameworkPropertyMetadataOptions.AffectsRender), 
                GridVisibility_Validate);

        public GridVisibility VerticalGridVisibility
        {
            get { return (GridVisibility)GetValue(VerticalGridVisibilityProperty); }
            set { SetValue(VerticalGridVisibilityProperty, value); }
        }

        private static bool GridVisibility_Validate(object value)
        {
            GridVisibility x = (GridVisibility)value;
            return (x == GridVisibility.ALLTICKS || x == GridVisibility.LONGTICKS || x == GridVisibility.HIDDEN);
        }
        #endregion

        #region Refresh

        private VisualContext visualContext = null;
        private VisualContext GetVisualContext()
        {
            if (visualContext == null)
            {
                visualContext = new VisualContext();
            }
            visualContext.Sources = this.DataSeries;
            return visualContext;
        }

        public void Refresh()
        {
            StartRefreshWorker();
        }

        private bool continueRefresh = false;
        private BackgroundWorker refreshWorker = null;
        private void StartRefreshWorker()
        {
            continueRefresh = true;
            if (refreshWorker == null)
            {
                refreshWorker = new BackgroundWorker();
                refreshWorker.DoWork += RefreshWorker_DoWork;
                refreshWorker.RunWorkerCompleted += RefreshWorker_RunWorkerCompleted;
            }
            if(!refreshWorker.IsBusy)
            {
                continueRefresh = false;
                refreshWorker.RunWorkerAsync();
            }
        }

        private void RefreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (continueRefresh && refreshWorker != null)
            {
                continueRefresh = false;
                refreshWorker.RunWorkerAsync();
            }
        }

        private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                UpdatePloterSize();

                UpdatePlotAreaBorder(plotterAreaBorder);

                CalculateAxisRangeData();

                UpdateAxis(horizAxesContainer);

                UpdateAxis(verticalAxesContainer);

                UpdateAxisGrid(axesGrid);

                UpdateDataSeries(plotterArea, this.GetVisualContext());

            }));
            System.Threading.Thread.Sleep(100);
        }

        private void UpdatePlotAreaBorder(DependencyObject host)
        {
            PlotterAreaBorder plotAreaBorder = host as PlotterAreaBorder;
            if (plotAreaBorder != null)
            {
                plotAreaBorder.Update();
            }
            if (host != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(host); ++i)
                {
                    DependencyObject item = VisualTreeHelper.GetChild(host, i);
                    UpdateAxisGrid(item);
                }
            }
        }

        private void CalculateAxisRangeData()
        {
            if (DataSeries != null)
            {
                foreach (var series in DataSeries)
                {
                    if (series != null && series.Points != null)
                    {
                        series.Points.Initialize(series);
                    }
                }
            }

            if (this.XAxes != null)
            {
                foreach (var xaxis in this.XAxes)
                {
                    List<DataSeries> list = null;
                    if (this.DataSeries != null)
                    {
                        list = (from dataSeries in this.DataSeries
                                where dataSeries.XAxis == xaxis
                                select dataSeries).ToList();
                    }
                    xaxis.CalculateRange(list);
                }
            }
            if (this.YAxes != null)
            {
                foreach (var yaxis in this.YAxes)
                {
                    List<DataSeries> list = null;
                    if (this.DataSeries != null)
                    {
                        list = (from dataSeries in this.DataSeries
                                where dataSeries.YAxis == yaxis
                                select dataSeries).ToList();
                    }
                    yaxis.CalculateRange(list);
                }
            }
        }

        private void UpdateAxis(DependencyObject host)
        {
            if (host != null)
            {
                var axis = host as IUpdate;
                if (axis != null)
                {
                    axis.Update();
                }
                if (host != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(host); ++i)
                    {
                        DependencyObject item = VisualTreeHelper.GetChild(host, i);
                        UpdateAxis(item);
                    }
                }
            }
        }

        private void UpdateAxisGrid(DependencyObject host)
        {
            IUpdate multiAxisGrid = host as IUpdate;
            if (multiAxisGrid != null)
            {
                multiAxisGrid.Update();
            }
            if (host != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(host); ++i)
                {
                    DependencyObject item = VisualTreeHelper.GetChild(host, i);
                    UpdateAxisGrid(item);
                }
            }
        }

        private void UpdateDataSeries(DependencyObject host, VisualContext visualContext)
        {
            if (host != null)
            {
                ISeriesRender sourceView = host as ISeriesRender;
                if (sourceView != null)
                {
                    sourceView.Render(visualContext);
                }
                if (host != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(host); ++i)
                    {
                        DependencyObject item = VisualTreeHelper.GetChild(host, i);
                        UpdateDataSeries(item, visualContext);
                    }
                }
            }
        }

        #endregion

        #region YAxisZone

        public static readonly DependencyProperty YAxisZoneProperty
            = DependencyProperty.Register("YAxisZone", typeof(AxisZoneTypes), typeof(Chart), 
                new FrameworkPropertyMetadata(AxisZoneTypes.EDITZONE, 
                    FrameworkPropertyMetadataOptions.AffectsRender | 
                    FrameworkPropertyMetadataOptions.AffectsMeasure, 
                    new PropertyChangedCallback(OnYAxisZoneChanged)));

        public AxisZoneTypes YAxisZone
        {
            get { return (AxisZoneTypes)GetValue(YAxisZoneProperty); }
            set { SetValue(YAxisZoneProperty, value); }
        }

        private static void OnYAxisZoneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            if (chart != null)
            {
                chart.UpdatePloterSize();
            }
        }

        public void ReleaseYAxisZone()
        {
            if (this.YAxes != null)
            {
                foreach (var axis in this.YAxes)
                {
                    if (axis != null)
                    {
                        axis.TopLeftPosScale = 0;
                        axis.BottomRightPosScale = 1;
                    }
                }
            }
            UpdatePloterSize();
        }

        #endregion

        #region X Axes
        
        public static readonly DependencyProperty XAxesProperty =
           DependencyProperty.Register("XAxes", typeof(ObservableCollection<Axis>),
           typeof(Chart), new FrameworkPropertyMetadata(null, 
               FrameworkPropertyMetadataOptions.AffectsArrange | 
               FrameworkPropertyMetadataOptions.AffectsMeasure | 
               FrameworkPropertyMetadataOptions.AffectsRender,
               new PropertyChangedCallback(OnXAxesPropertyChanged)));

        public ObservableCollection<Axis> XAxes
        {
            get { return (ObservableCollection<Axis>)GetValue(XAxesProperty); }
            set { SetValue(XAxesProperty, value); }
        }

        private static void OnXAxesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            if (chart != null)
            {
                if (e.OldValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= chart.XAxes_CollectionChanged;
                    }
                    var xAxes = e.OldValue as IList<Axis>;
                    if (xAxes != null)
                    {
                        foreach (var x in xAxes)
                        {
                            x.PropertyChanged -= chart.Axes_PropertyChanged;
                        }
                    }
                }
                if (e.NewValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged += chart.XAxes_CollectionChanged;
                    }

                    var xAxes = e.NewValue as IList<Axis>;
                    if (xAxes != null)
                    {
                        foreach (var x in xAxes)
                        {
                            x.PropertyChanged += chart.Axes_PropertyChanged;
                            chart.UpdateXAxisSize(x);
                        }
                    }
                }
                chart.RaiseNotifyPropertyChanged("XAxes");
            }
        }

        private void XAxes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var xAxis = item as Axis;
                    if (xAxis != null)
                    {
                        xAxis.PropertyChanged -= Axes_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var xAxis = item as Axis;
                    if (xAxis != null)
                    {
                        xAxis.PropertyChanged += Axes_PropertyChanged;
                        UpdateXAxisSize(xAxis);
                    }
                }
            }
            RaiseNotifyPropertyChanged("XAxes");
        }

        private void UpdateXAxisSize(Axis xaxis)
        {
            if (xaxis != null)
            {
                if (this.PlotterSize != null && this.PlotterSize.Width > 0.0)
                {
                    xaxis.Extent = this.PlotterSize.Width;
                }
            }
        }

        private void Axes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.YAxisZone == AxisZoneTypes.EDITZONE)
            {
                Refresh();
            }
        }

        #endregion

        #region Y Axes

        public static readonly DependencyProperty YAxesProperty =
           DependencyProperty.Register("YAxes", typeof(ObservableCollection<Axis>),
           typeof(Chart), new FrameworkPropertyMetadata(null,
               FrameworkPropertyMetadataOptions.AffectsArrange |
               FrameworkPropertyMetadataOptions.AffectsMeasure |
               FrameworkPropertyMetadataOptions.AffectsRender,
               new PropertyChangedCallback(OnYAxesPropertyChanged)));

        public ObservableCollection<Axis> YAxes
        {
            get { return (ObservableCollection<Axis>)GetValue(YAxesProperty); }
            set { SetValue(YAxesProperty, value); }
        }

        private static void OnYAxesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart chart = d as Chart;
            if (chart != null)
            {
                if (e.OldValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= chart.YAxes_CollectionChanged;
                    }
                    var yAxes = e.OldValue as IList<Axis>;
                    if (yAxes != null)
                    {
                        foreach (var y in yAxes)
                        {
                            y.PropertyChanged -= chart.Axes_PropertyChanged;
                        }
                    }
                }
                if (e.NewValue != null)
                {
                    INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged += chart.YAxes_CollectionChanged;
                    }

                    var yAxes = e.NewValue as IList<Axis>;
                    if (yAxes != null)
                    {
                        foreach (var y in yAxes)
                        {
                            y.PropertyChanged += chart.Axes_PropertyChanged;
                            chart.UpdateYAxisSize(y);
                        }
                    }
                }
                chart.RaiseNotifyPropertyChanged("YAxes");
            }
        }

        private void YAxes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var yAxis = item as Axis;
                    if (yAxis != null)
                    {
                        yAxis.PropertyChanged -= Axes_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var yAxis = item as Axis;
                    if (yAxis != null)
                    {
                        yAxis.PropertyChanged += Axes_PropertyChanged;
                        UpdateYAxisSize(yAxis);
                    }
                }
            }
            RaiseNotifyPropertyChanged("XAxes");
        }

        private void UpdateYAxisSize(Axis yaxis)
        {
            if (yaxis != null)
            {
                if (this.PlotterSize != null && this.PlotterSize.Height > 0.0)
                {
                    yaxis.Extent = this.PlotterSize.Height;
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaiseNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
