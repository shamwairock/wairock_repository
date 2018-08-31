using System;
using WpfTreeView2.Interfaces;
using System.Collections.ObjectModel;


namespace WpfTreeView2.Models
{
    public class Menu : IUiElement
    {
        #region Property
        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
            }
        }

        //private string _toolTip;
        //public string ToolTip
        //{
        //    get { return _toolTip; }
        //    set { _toolTip = value; }
        //}

        private ObservableCollection<IUiElement> _menuItems;
        public ObservableCollection<IUiElement> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; }
        }

        //private string _nodePath;
        //public string NodePath
        //{
        //    get { return _nodePath; }
        //    set { _nodePath = value; }
        //}
        #endregion

        public Menu()
        {
            MenuItems = new ObservableCollection<IUiElement>();
        }

        //private RelayCommand selectedItemChangedCommand;

        //public RelayCommand SelectedItemChangedCommand
        //    => selectedItemChangedCommand ?? (selectedItemChangedCommand = new RelayCommand(SelectedItemChanged));

        //private void SelectedItemChanged(object obj)
        //{
        //    var selectedMenu = obj as IMenu;
        //    if (selectedMenu != null)
        //    {
        //    }
        //}

        //public IUiElement GetUiElement(string nodePath)
        //{
        //    IUiElement foundMenu = null;
        //    if (string.IsNullOrEmpty(nodePath))
        //    {
        //        throw new ArgumentNullException(nameof(nodePath));
        //    }

        //    if (nodePath == _nodePath)
        //    {
        //        return this;
        //    }

        //    foreach (var uiElement in Menus)
        //    {
        //        var menu = uiElement as IMenu;
        //        if (nodePath == _nodePath)
        //        {
        //            foundMenu = menu;
        //            break;
        //        }

        //        if (menu != null) foundMenu = menu.GetUiElement(nodePath);

        //        if (foundMenu != null)
        //        {
        //            break;
        //        }
        //    }

        //    return foundMenu;
        //}
    }
}
