using System.Collections.ObjectModel;

namespace WpfTreeView
{
    public class MenuTree : BaseViewModel, IMenuTree
    {
        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        private ObservableCollection<IMenuTree> _menuTrees;

        public ObservableCollection<IMenuTree> MenuTrees
        {
            get
            {
                if (_menuTrees == null)
                {
                    _menuTrees = new ObservableCollection<IMenuTree>();
                }
                return _menuTrees;
            }
            set
            {
                _menuTrees = value;
                OnPropertyChanged("MenuTrees");
            }
        }

        private ObservableCollection<IParameter> _parameters;

        public ObservableCollection<IParameter> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new ObservableCollection<IParameter>();
                }
                return _parameters;
            }
            set
            {
                _parameters = value;
                OnPropertyChanged("Parameters");
            }
        }
    }
}