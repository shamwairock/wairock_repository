using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuViewControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var menuView = new MenuView();
            var menuViewModel = new MenuViewModel
            {
                Name = "Root Menu",
                Menus = new ObservableCollection<Menu>()
                {
                    new Menu() {Name = "Menu 1"},
                    new Menu() {Name = "Menu 2"},
                    new Menu() {Name = "Menu 3"}
                }
            };
            menuView.DataContext = menuViewModel;

            MyGrid.Children.Add(menuView);
        }
    }
}
