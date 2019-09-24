using System;
using System.Collections.Generic;
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

namespace TabViewTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tabView = new TabView();
            
            var tabItemView1 = new TabItemView();
            var tabItemView2 = new TabItemView();

            tabView.Items.Add(tabItemView1);
            tabView.Items.Add(tabItemView2);


            MyGrid.Children.Add(tabView);
        }
    }
}
