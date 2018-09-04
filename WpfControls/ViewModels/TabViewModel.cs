using System.Collections.ObjectModel;

namespace WpfControls
{
    public class TabViewModel : UiModel
    {
        public ObservableCollection<TabItemViewModel> TabItemViewModels { get; set; }
    }
}
