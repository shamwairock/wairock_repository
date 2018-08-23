using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfTreeView
{
    public class Family : INode
    {
        public Family()
        {
            this.Members = new ObservableCollection<INode>();
        }

        public string Name { get; set; }
        public ObservableCollection<INode> Members { get; set; }
        public Family Parent { get; private set; }
    }

}
