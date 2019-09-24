using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MenuViewControl
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
    }
}
