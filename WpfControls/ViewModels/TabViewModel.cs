using System.Collections.ObjectModel;
using System.Linq;

namespace WpfControls
{
    public class TabViewModel : UiModel
    {
        public ObservableCollection<TabItemViewModel> TabItemViewModels { get; set; }


        private TabItemViewModel selectedTabItemViewModel;

        public TabItemViewModel SelectedTabItemViewModel
        {
            get
            {
                if (selectedTabItemViewModel == null && TabItemViewModels !=null && TabItemViewModels.Count >0)
                {
                    selectedTabItemViewModel = TabItemViewModels.First();
                }
                return selectedTabItemViewModel;
            }
            set
            {
                OnPropertyChanged("SelectedTabItemViewModel");
                selectedTabItemViewModel = value;
            }
        }

        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand => selectionChangedCommand ?? new RelayCommand(OnSelectionChangedCommand);

        private void OnSelectionChangedCommand(object obj)
        {
            var selectedTabViewModel = obj as TabItemViewModel;
            
        }
    }
}
