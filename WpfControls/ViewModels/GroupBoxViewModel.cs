using System.Collections.ObjectModel;

namespace WpfControls
{
    public class GroupBoxViewModel : UiModel
    {
        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        public ObservableCollection<UiModel> UiModels { get; set; }
    }
}
