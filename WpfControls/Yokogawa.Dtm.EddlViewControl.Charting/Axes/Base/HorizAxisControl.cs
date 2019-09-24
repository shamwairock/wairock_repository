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

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class HorizAxisControl : AxisControl
    {
        public static readonly DependencyProperty AxisMarginProperty
            = DependencyProperty.Register("AxisMargin", typeof(Thickness), typeof(HorizAxisControl),
                new PropertyMetadata(new Thickness(),new PropertyChangedCallback(OnAxisMarginChanged)));

        public Thickness AxisMargin
        {
            get { return (Thickness)GetValue(AxisMarginProperty); }
            set { SetValue(AxisMarginProperty, value); }
        }
        private static void OnAxisMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
