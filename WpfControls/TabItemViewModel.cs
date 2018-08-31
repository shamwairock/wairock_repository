using System.Collections.ObjectModel;

namespace WpfControls
{
    public class TabItemViewModel : BaseViewModel
    {
        public ObservableCollection<ListViewItemModel> ListViewItemModels { get; set; }
    }
}
