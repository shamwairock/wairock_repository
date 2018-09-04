using System.Collections.ObjectModel;

namespace WpfControls
{
    public class TabItemViewModel : UiModel
    {
        public ObservableCollection<UiModel> UiModels { get; set; }
        public string Label { get; set; }
    }
}
