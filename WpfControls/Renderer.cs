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
                tabItemViewModel.ListViewItemModels = new ObservableCollection<ControlModel>();
                var datePickerVm = new DatePickerControlViewModel();
                datePickerVm.Label = "I am datePicker";
                
                var stringVm = new StringControlViewModel();
                stringVm.Label = "I am String";
                stringVm.Value = "default";

                var comboBoxVm = new EnumControlViewModel();
                comboBoxVm.Label = "I am a combobox";
                comboBoxVm.Selections  = new ObservableCollection<IEnumeration>();
                comboBoxVm.Selections.Add(new Enumeration() { Label = "kg/m3", Id = "1" });
                comboBoxVm.Selections.Add(new Enumeration() { Label = "mmHg", Id = "2" });
                comboBoxVm.SelectedItem = comboBoxVm.Selections.First();

                var groupBox = new GroupBoxControlViewModel();
                groupBox.Label = "I am just a groupbox";
                groupBox.ListViewItemModels = new ObservableCollection<ControlModel>();
                groupBox.ListViewItemModels.Add(datePickerVm);
                groupBox.ListViewItemModels.Add(stringVm);
                groupBox.ListViewItemModels.Add(comboBoxVm);

                var bitEnumViewModel =  new BitEnumControlViewModel();
                bitEnumViewModel.Label = "I am a bit enum";
                bitEnumViewModel.StrCheckedItems = "Selected values";
                
                tabItemViewModel.ListViewItemModels.Add(datePickerVm);
                tabItemViewModel.ListViewItemModels.Add(stringVm);
                tabItemViewModel.ListViewItemModels.Add(comboBoxVm);
                tabItemViewModel.ListViewItemModels.Add(groupBox);
                tabItemViewModel.ListViewItemModels.Add(bitEnumViewModel);

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
