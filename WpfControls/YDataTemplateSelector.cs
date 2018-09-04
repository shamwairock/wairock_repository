using System.Windows;
using System.Windows.Controls;
using WpfControls.ViewModels;

namespace WpfControls
{
    public class YDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DatePickerViewDataTemplate { get; set; }
        public DataTemplate StringViewDataTemplate { get; set; }
        public DataTemplate GroupBoxViewDataTemplate { get; set; }
        public DataTemplate EnumViewDataTemplate { get; set; }
        public DataTemplate BitEnumViewDataTemplate { get; set; }
        public DataTemplate TreeViewDataTemplate { get; set; }

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

            var dateViewModel = item as DatePickerViewModel;
            if (dateViewModel != null)
            {
                DatePickerViewDataTemplate = frameworkElement.FindResource("datePickerViewDataTemplate") as DataTemplate;
                return DatePickerViewDataTemplate;
            }

            var stringViewModel = item as StringViewModel;
            if (stringViewModel != null)
            {
                StringViewDataTemplate = frameworkElement.FindResource("stringViewDataTemplate") as DataTemplate;
                return StringViewDataTemplate;
            }

            var groupViewModel = item as GroupBoxViewModel;
            if (groupViewModel != null)
            {
                GroupBoxViewDataTemplate = frameworkElement.FindResource("groupBoxViewDataTemplate") as DataTemplate;
                return GroupBoxViewDataTemplate;
            }

            var enumControlViewModel = item as EnumViewModel;
            if (enumControlViewModel != null)
            {
                EnumViewDataTemplate = frameworkElement.FindResource("enumViewDataTemplate") as DataTemplate;
                return EnumViewDataTemplate;
            }

            var bitEnumControlViewModel = item as BitEnumViewModel;
            if (bitEnumControlViewModel != null)
            {
                BitEnumViewDataTemplate = frameworkElement.FindResource("bitEnumViewDataTemplate") as DataTemplate;
                return BitEnumViewDataTemplate;
            }

            var treeViewModel = item as TreeViewModel;
            if (treeViewModel != null)
            {
                TreeViewDataTemplate = frameworkElement.FindResource("treeViewDataTemplate") as DataTemplate;
                return TreeViewDataTemplate;
            }


            return null;
        }
    }
}
