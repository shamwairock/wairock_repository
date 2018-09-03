using System.ComponentModel;
using WpfControls.Annotations;

namespace WpfControls
{
    public class BitEnumeration : BaseViewModel, IBitEnumeration
    {
        private string label;

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        private string id;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
