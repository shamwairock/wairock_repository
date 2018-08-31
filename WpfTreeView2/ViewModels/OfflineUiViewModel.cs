using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfTreeView2.Interfaces;
using WpfTreeView2.Models;

namespace WpfTreeView2.ViewModels
{
    public class OfflineUiViewModel : BaseViewModel
    {
        
        private IUiElement _rootMenu;

        private ObservableCollection<IUiElement> _uiElements;
        public ObservableCollection<IUiElement> UiElements
        {
            get
            {
                return _uiElements;
            }
            set
            {
                _uiElements = value;
                OnPropertyChanged("UiElements");
            }
        }

        public OfflineUiViewModel()
        {
         
            CreateUidLayout();
        }

        public void CreateUidLayout()
        {
            try
            {
                UiElements = new ObservableCollection<IUiElement>();

                var menu1 = new Menu() {Label = "Menu1"};
                menu1.MenuItems = new ObservableCollection<IUiElement>();
                menu1.MenuItems.Add(new Variable("NodePath1", "Var1", "Var1"));
                menu1.MenuItems.Add(new Variable("NodePath2", "Var2", "Var2"));
                UiElements.Add(menu1);

                //UiElements.Add(new Menu() {Label = "menu1"});
                //UiElements.Add(new Variable("path1", "label1", "tooltip1"));

            }
            catch (Exception ex)
            {
                
            }
        }

        //private IUiElement GetUiElement(string nodePath)
        //{
        //    IUiElement uiElementResult = null;

        //    foreach (var uiElement in UiElements)
        //    {
        //        var menu = uiElement as IMenu;
        //        if (menu != null) uiElementResult = menu.GetUiElement(nodePath);
        //    }

        //    return uiElementResult;
        //}
    }
}
