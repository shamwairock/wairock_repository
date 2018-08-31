using System.Collections.ObjectModel;

namespace WpfControls
{
    public class EnumControlViewModel<T> : ListViewItemModel
    {
        private ObservableCollection<T> _selections;

        public ObservableCollection<T> Selections
        {
            get { return _selections; }
            set
            {
                _selections = value;
            }
        }


        private T _selectedItem;

        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public EnumControlViewModel<T> Clone()
        {
            return new EnumControlViewModel<T>();
        }
    }
}
