using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfTreeView
{
    public class MainWindowVm : BaseViewModel
    {
        private ObservableCollection<INode> families;
        public ObservableCollection<INode> Families
        {
            get { return families; }
            set
            {
                families = value;
                OnPropertyChanged("Families");
            }
        }
        public MainWindowVm()
        {
            // FAMILIES
            Families = new ObservableCollection<INode>();

            Family family1 = new Family() { Name = "The Doe's" };
            family1.Members.Add(new FamilyMember() { Name = "John Doe", Age = 42 });
            family1.Members.Add(new FamilyMember() { Name = "Jane Doe", Age = 39 });
            family1.Members.Add(new FamilyMember() { Name = "Sammy Doe", Age = 13 });
            Families.Add(family1);

            Family family2 = new Family() { Name = "The Moe's" };
            family2.Members.Add(new FamilyMember() { Name = "Mark Moe", Age = 31 });
            family2.Members.Add(new FamilyMember() { Name = "Norma Moe", Age = 28 });
            Families.Add(family2);

            Family family3 = new Family() { Name = "The Dunkin's" };
            family3.Members.Add(new FamilyMember() { Name = "Kevin Dunkin", Age = 31 });
            family3.Members.Add(new FamilyMember() { Name = "Breana Dunkin", Age = 28 });
            family2.Members.Add(family3);
        }
    }
}
