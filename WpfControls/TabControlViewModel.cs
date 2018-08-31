using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfControls
{
    public class TabControlViewModel : BaseViewModel
    {
        public ObservableCollection<TabItemViewModel> TabItemViewModels { get; set; }
    }
}
