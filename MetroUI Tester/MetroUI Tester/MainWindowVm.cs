using System;
using System.ComponentModel;
namespace MetroUI_Tester
{
    public class MainWindowVm : INotifyPropertyChanged , IDataErrorInfo
    {
        string _PV;
        public string PV
        {
            get { return this._PV; }
            set
            {
                if (Equals(value, _PV))
                {
                    return;
                }

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
                    if (PV == "100")
                        return "Yes knock off!";
                }

                return null;
            }
        }


        [Description("Test-Property")]
        public string Error => string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
