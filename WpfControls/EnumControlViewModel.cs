using System.Collections.ObjectModel;

namespace WpfControls
{
    public class EnumControlViewModel : ControlModel
    {
        private ObservableCollection<IEnumeration> _selections;

        public ObservableCollection<IEnumeration> Selections
        {
            get { return _selections; }
            set
            {
                _selections = value;
            }
        }


        private IEnumeration _selectedItem;

        public IEnumeration SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
    }
}
