using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    public class YDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DatePickerViewDataTemplate { get; set; }
        public DataTemplate StringViewDataTemplate { get; set; }
        public DataTemplate GroupBoxViewDataTemplate { get; set; }
        public DataTemplate EnumViewDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            FrameworkElement frameworkElement  = container as FrameworkElement;
            if (frameworkElement == null)
            {
                return null;
            }

            var dateViewModel = item as DatePickerControlViewModel;
            if (dateViewModel != null)
            {
                DatePickerViewDataTemplate = frameworkElement.FindResource("datePickerViewDataTemplate") as DataTemplate;
                return DatePickerViewDataTemplate;
            }

            var stringViewModel = item as StringControlViewModel;
            if (stringViewModel != null)
            {
                StringViewDataTemplate = frameworkElement.FindResource("stringViewDataTemplate") as DataTemplate;
                return StringViewDataTemplate;
            }

            var groupViewModel = item as GroupBoxControlViewModel;
            if (groupViewModel != null)
            {
                GroupBoxViewDataTemplate = frameworkElement.FindResource("groupBoxViewDataTemplate") as DataTemplate;
                return GroupBoxViewDataTemplate;
            }

            var comboBoxViewModel = item as EnumControlViewModel<IRelation>;
            if (comboBoxViewModel != null)
            {
                EnumViewDataTemplate = frameworkElement.FindResource("enumViewDataTemplate") as DataTemplate;
                return EnumViewDataTemplate;
            }

            return null;
        }
    }
}
