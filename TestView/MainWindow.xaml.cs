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

namespace TestView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TabView tabView1;
        private TabView tabView2;

        public MainWindow()
        {
            InitializeComponent();

            var grid = (Grid)FindName("ContentDocker");
            if (grid != null)
            {
                grid.Children.Clear();

                tabView1 = new TabView();
                tabView1.Visibility = Visibility.Visible;
                tabView1.Items.Add(new TabItem() { Header = "Tab1" });

                tabView2 = new TabView();
                tabView2.Visibility = Visibility.Visible;
                tabView2.Items.Add(new TabItem() { Header = "Tab2" });

                grid.Children.Add(tabView1);
                grid.Children.Add(tabView2);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tabView2.Visibility = Visibility.Collapsed;
            tabView1.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tabView1.Visibility = Visibility.Collapsed;
            tabView2.Visibility = Visibility.Visible;
        }
    }
}
