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

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowVm();
            //vm.MenuTree = new MenuTree();
            //vm.MenuTree.Label = "Root";
            //vm.MenuTree.MenuTrees = new ObservableCollection<IMenuTree>();
            //vm.MenuTree.MenuTrees.Add(new MenuTree() {Label = "Child 1"});
            //vm.MenuTree.MenuTrees.Add(new MenuTree() { Label = "Child 2" });
            //vm.MenuTree.MenuTrees.First().MenuTrees = new ObservableCollection<IMenuTree>();
            //vm.MenuTree.MenuTrees.First().MenuTrees.Add(new MenuTree() { Label = "Child 1.1" });
            //this.DataContext = vm;

            vm.Children = new ObservableCollection<IMenuTree>();
            vm.Children.Add(new MenuTree() {Label = "1"});
            vm.Children.Add(new MenuTree() { Label = "2" });
            vm.Children.Add(new MenuTree() { Label = "3" });

            var first = vm.Children[0];
            var firstItem = first as IMenuTree;
            firstItem.MenuTrees.Add(new MenuTree() {Label="Tree 1"});
            firstItem.MenuTrees.Add(new MenuTree() { Label = "Tree 2" });
            firstItem.MenuTrees.Add(new MenuTree() { Label = "Tree 3" });

            firstItem.MenuTrees[1].Parameters.Add(new Parameter() {Label="Parameter 1"});
            firstItem.MenuTrees[1].Parameters.Add(new Parameter() { Label = "Parameter 2" });
            firstItem.MenuTrees[1].Parameters.Add(new Parameter() { Label = "Parameter 3" });

            this.DataContext = vm;

        }
    }
}
