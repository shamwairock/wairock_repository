using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sliding_Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool menuShown;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowHideMenu(string storyBoard, Button btnHide, Button btnShow, StackPanel pnl)
        {
            Storyboard sb = Resources[storyBoard] as Storyboard;
            sb.Begin(pnl);

            if (storyBoard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (storyBoard.Contains("Hide"))
            {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!menuShown)
            {
                Storyboard sb = Resources["sbShowLeftMenu"] as Storyboard;
                sb.Begin(pnlLeftMenu);

                menuShown = true;
            }
            else
            {
                Storyboard sb = Resources["sbHideLeftMenu"] as Storyboard;
                sb.Begin(pnlLeftMenu);

                menuShown = false;
            }
        }
    }
}
