using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Effects;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Yokogawa.Dtm.EddlViewControl.Charting.PlotArea"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Yokogawa.Dtm.EddlViewControl.Charting.PlotArea;assembly=Yokogawa.Dtm.EddlViewControl.Charting.PlotArea"
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
    ///     <MyNamespace:PlotterArea/>
    ///
    /// </summary>
    public class PlotterArea : Control
    {
        static PlotterArea()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotterArea), new FrameworkPropertyMetadata(typeof(PlotterArea)));
        }

        public event PlotterAreaSizeChangedEvent PlotterAreaSizeChangedEvent;
        private void RaisePlotterAreaSizeChangedEvent(PlotterAreaSizeChangedEventArgs e)
        {
            if (this.PlotterAreaSizeChangedEvent != null)
            {
                PlotterAreaSizeChangedEvent(this, e);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            var args = new PlotterAreaSizeChangedEventArgs()
            {
                Size = sizeInfo.NewSize,
                WidthChanged = sizeInfo.WidthChanged,
                HeightChanged = sizeInfo.HeightChanged,
            };
            RaisePlotterAreaSizeChangedEvent(args);
        }

        #region DataSeries
        public ObservableCollection<DataSeries> DataSeries
        {
            get { return (ObservableCollection<DataSeries>)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }

        public static readonly DependencyProperty DataSeriesProperty =
           DependencyProperty.Register("DataSeries", 
           typeof(ObservableCollection<DataSeries>),
           typeof(PlotterArea),
           new FrameworkPropertyMetadata(null,
               FrameworkPropertyMetadataOptions.AffectsArrange |
               FrameworkPropertyMetadataOptions.AffectsMeasure |
               FrameworkPropertyMetadataOptions.AffectsRender,
               new PropertyChangedCallback(OnDataSeriesPropertyChanged)));

        private static void OnDataSeriesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region CurveSelectedEffect

        public static readonly DependencyProperty CurveSelectedEffectProperty
           = DependencyProperty.Register("CurveSelectedEffect", 
           typeof(Effect), 
           typeof(PlotterArea), 
           new FrameworkPropertyMetadata(new DropShadowEffect() { BlurRadius = 2, Direction = 322, ShadowDepth = 5, Opacity = 0.5 }, 
               FrameworkPropertyMetadataOptions.AffectsMeasure | 
               FrameworkPropertyMetadataOptions.AffectsRender | 
               FrameworkPropertyMetadataOptions.Inherits));

        public Effect CurveSelectedEffect
        {
            get { return (Effect)GetValue(CurveSelectedEffectProperty); }
            set { SetValue(CurveSelectedEffectProperty, value); }
        }

        #endregion
    }
}
