using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfControls
{
    public class StringControlViewModel : ListViewItemModel
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
