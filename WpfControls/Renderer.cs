using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfControls.ViewModels;

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

                var tabControlView = new TabView();
                var tabControlViewModel = new TabViewModel();

                tabControlViewModel.TabItemViewModels = new ObservableCollection<TabItemViewModel>();
                var tabItemViewModel = new TabItemViewModel();
                tabItemViewModel.Label = "Tab Item 1";
                tabItemViewModel.UiModels = new ObservableCollection<UiModel>();
                var datePickerVm = new DatePickerViewModel();
                datePickerVm.Label = "I am datePicker";
                
                var stringVm = new StringViewModel();
                stringVm.Label = "I am String";
                stringVm.Value = "1234567890";
           

                var comboBoxVm = new EnumViewModel();
                comboBoxVm.Label = "I am a combobox";
                comboBoxVm.Selections  = new ObservableCollection<IEnumeration>();
                comboBoxVm.Selections.Add(new Enumeration() { Label = "kg/m3", Id = "1" });
                comboBoxVm.Selections.Add(new Enumeration() { Label = "mmHg", Id = "2" });
                comboBoxVm.SelectedItem = comboBoxVm.Selections.First();

                var groupBox = new GroupBoxViewModel();
                groupBox.Label = "I am just a groupbox";
                groupBox.UiModels = new ObservableCollection<UiModel>();
                groupBox.UiModels.Add(datePickerVm);
                groupBox.UiModels.Add(stringVm);
                groupBox.UiModels.Add(comboBoxVm);

                var bitEnumViewModel =  new BitEnumViewModel();
                bitEnumViewModel.Label = "I am a bit enum";
                bitEnumViewModel.StrCheckedItems = "Selected values";
                
                var tree = new TreeViewModel();
                tree.Label = "Root";
                tree.TreeNodes = new ObservableCollection<TreeNode>();
                tree.TreeNodes.Add(new TreeNode() {Id="1", Label="Node1"});

                var memberNode = new TreeNode() {Id = "2", Label = "Node2"};
                memberNode.TreeNodes = new ObservableCollection<TreeNode>();
                memberNode.TreeNodes.Add(new TreeNode() { Id = "1", Label = "Node3" });
                tree.TreeNodes.Add(memberNode);

                tabItemViewModel.UiModels.Add(datePickerVm);
                tabItemViewModel.UiModels.Add(stringVm);
                tabItemViewModel.UiModels.Add(comboBoxVm);
                tabItemViewModel.UiModels.Add(groupBox);
                tabItemViewModel.UiModels.Add(bitEnumViewModel);
                tabItemViewModel.UiModels.Add(tree);

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
