using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicYAxisControl : VerticalAxisControl
    {
        public LogarithmicYAxisControl()
        {
            var itemVisual = new LogarithmicAxisVisual();

            BindingOperations.SetBinding(itemVisual, AxisProperty, new Binding("Axis") { Source = this });
            BindingOperations.SetBinding(itemVisual, FontFamilyProperty, new Binding("FontFamily") { Source = this });
            BindingOperations.SetBinding(itemVisual, FontSizeProperty, new Binding("FontSize") { Source = this });
            BindingOperations.SetBinding(itemVisual, FontStyleProperty, new Binding("FontStyle") { Source = this });
            BindingOperations.SetBinding(itemVisual, FontWeightProperty, new Binding("FontWeight") { Source = this });
            BindingOperations.SetBinding(itemVisual, FontStretchProperty, new Binding("FontStretch") { Source = this });
            BindingOperations.SetBinding(itemVisual, ContentLayoutProperty, new Binding("ContentLayout") { Source = this });
            BindingOperations.SetBinding(itemVisual, TickLabelFontSizeProperty, new Binding("TickLabelFontSize") { Source = this });
            BindingOperations.SetBinding(itemVisual, AxisMarginProperty, new Binding("AxisMargin") { Source = this });

            visuals.Add(itemVisual);
        }
    }
}
