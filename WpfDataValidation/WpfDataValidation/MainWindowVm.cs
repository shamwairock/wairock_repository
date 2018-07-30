using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfDataValidation
{
    public class MainWindowVm : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _PV;

        public string PV
        {
            get { return _PV; }
            set
            {
                _PV = value;
                RaisePropertyChanged("PV");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "PV")
                {
                    return ValidatePV(PV);
                }
                
                return null;
            }
        }

        public string Error => string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        private string ValidatePV(string input)
        {
            if(input == "100")
            {
                return "I am 100. Not Good!";
            }

            return null;
        }
    }
}
