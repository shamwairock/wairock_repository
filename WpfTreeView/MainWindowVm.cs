using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfTreeView
{
    public class MainWindowVm : BaseViewModel
    {
        //private IMenuTree _menuTree;

        //public IMenuTree MenuTree
        //{
        //    get
        //    {
        //        if (_menuTree == null)
        //        {
        //            _menuTree = new MenuTree();
        //        }
        //        return _menuTree;
        //    }
        //    set
        //    {
        //        _menuTree = value;
        //        OnPropertyChanged("MenuTree");
        //    }
        //}

        private IList children = new CompositeCollection() {
                new CollectionContainer { Collection = new List<IMenuTree>() },
                new CollectionContainer { Collection = new List<IParameter>() }
        };

        public IList Children
        {
            get { return children; }
            set { children = value; }
        }
    }
}
