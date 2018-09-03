using System.Collections.ObjectModel;

namespace WpfControls
{
    public class TabItemViewModel : BaseViewModel, ILabel
    {
        public ObservableCollection<ControlModel> ListViewItemModels { get; set; }
        public string Label { get; set; }
    }
}
