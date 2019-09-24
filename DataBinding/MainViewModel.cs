using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DataBinding.Annotations;

namespace DataBinding
{
    public class MainViewModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string displayValue = "Default";
        public string DisplayValue
        {
            get
            {
                return displayValue;
            }
            set
            {
                displayValue = value;
                OnPropertyChanged("DisplayValue");
            }
        }
    }
}
