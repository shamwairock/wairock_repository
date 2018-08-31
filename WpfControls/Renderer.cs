using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    public class Renderer
    {
        private FrameworkElement _element;
        public Renderer(FrameworkElement element)
        {
            _element = element;
        }

        public void CreateControls()
        {
            try
            {
                var mainGrid = _element as Grid;
                if (mainGrid == null) return;

                var tabControlView = new TabControlView();
                var tabControlViewModel = new TabControlViewModel();

                tabControlViewModel.TabItemViewModels = new ObservableCollection<TabItemViewModel>();
                var tabItemViewModel = new TabItemViewModel();
                tabItemViewModel.Label = "Tab Item 1";
                tabItemViewModel.ListViewItemModels = new ObservableCollection<ListViewItemModel>();
                var datePickerVm = new DatePickerControlViewModel();
                datePickerVm.Label = "I am datePicker";
                
                var stringVm = new StringControlViewModel();
                stringVm.Label = "I am String";
                stringVm.Value = "default";

                var comboBoxVm = new EnumControlViewModel<IRelation>();
                comboBoxVm.Label = "I am a combobox";
                comboBoxVm.Selections  = new ObservableCollection<IRelation>();
                comboBoxVm.Selections.Add(new SiUnit() {Label = "kg/m3"});
                comboBoxVm.Selections.Add(new SiUnit() { Label = "bar" });
                comboBoxVm.Selections.Add(new SiUnit() { Label = "mmHg" });
                comboBoxVm.SelectedItem = comboBoxVm.Selections.First();

                var groupBox = new GroupBoxControlViewModel();
                groupBox.Label = "I am just a groupbox";
                groupBox.ListViewItemModels = new ObservableCollection<ListViewItemModel>();
                groupBox.ListViewItemModels.Add(datePickerVm);
                groupBox.ListViewItemModels.Add(stringVm);
                groupBox.ListViewItemModels.Add(comboBoxVm);

                tabItemViewModel.ListViewItemModels.Add(datePickerVm);
                tabItemViewModel.ListViewItemModels.Add(stringVm);
                tabItemViewModel.ListViewItemModels.Add(comboBoxVm);
                tabItemViewModel.ListViewItemModels.Add(groupBox);

                tabControlViewModel.TabItemViewModels.Add(tabItemViewModel);
                tabControlView.DataContext = tabControlViewModel;

                mainGrid.Children.Add(tabControlView);
              
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
