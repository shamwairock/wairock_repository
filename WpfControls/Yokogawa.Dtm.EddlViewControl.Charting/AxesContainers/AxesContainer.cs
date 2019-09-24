using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class AxesContainer : Control
    {
        #region Axes
        public ObservableCollection<Axis> Axes
        {
            get { return (ObservableCollection<Axis>)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        public static readonly DependencyProperty AxesProperty =
            DependencyProperty.Register("Axes", typeof(ObservableCollection<Axis>), typeof(AxesContainer), 
                new PropertyMetadata(null, new PropertyChangedCallback(OnAxesPropertyChanged)));

        private static void OnAxesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO:
        }
        #endregion

        #region Zone

        public static readonly DependencyProperty AxisZoneProperty
            = DependencyProperty.Register("AxisZone", typeof(AxisZoneTypes), typeof(AxesContainer),
                new PropertyMetadata(AxisZoneTypes.EDITZONE, new PropertyChangedCallback(OnAxisZoneChanged)));

        public AxisZoneTypes AxisZone
        {
            get { return (AxisZoneTypes)GetValue(AxisZoneProperty); }
            set { SetValue(AxisZoneProperty, value); }
        }

        private static void OnAxisZoneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO:
        }
        #endregion

        #region AxisMargin

        public static readonly DependencyProperty AxisMarginProperty
            = DependencyProperty.Register("AxisMargin", typeof(Thickness), typeof(AxesContainer),
                new PropertyMetadata(new Thickness(), new PropertyChangedCallback(OnAxisMarginChanged)));

        public Thickness AxisMargin
        {
            get { return (Thickness)GetValue(AxisMarginProperty); }
            set { SetValue(AxisMarginProperty, value); }
        }

        private static void OnAxisMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO:
        }
        #endregion
    }
}
