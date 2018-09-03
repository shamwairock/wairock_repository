using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfControls
{
    public class BitEnumControlViewModel : ControlModel
    { 
        public ObservableCollection<IBitEnumeration> Selections { get; set; }

        private string _strCheckedItems;

        public string StrCheckedItems
        {
            get { return _strCheckedItems; }
            set
            {
                _strCheckedItems = value;
                OnPropertyChanged("StrCheckedItems");
            }
        }

        private string DisplayFormat(IBitEnumeration[] bitEnumerations)
        {
            if (bitEnumerations == null || bitEnumerations.Length == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < bitEnumerations.Length; i++)
            {
                sb.Append(i == bitEnumerations.Length - 1
                    ? $"{bitEnumerations[i].Label}"
                    : $"{bitEnumerations[i].Label},");
            }
            return sb.ToString();
        }

        public BitEnumControlViewModel()
        {
            if (Selections == null)
            {
                Selections = new ObservableCollection<IBitEnumeration>();

                for (int i = 0; i < 3; i++)
                {
                    var bitenumeration = new BitEnumeration();
                    bitenumeration.Id = (i + 1).ToString();
                    bitenumeration.Label = "Value " + (i + 1);
                    bitenumeration.PropertyChanged += Bitenumeration_PropertyChanged;
                    Selections.Add(bitenumeration);
                }
            }
        }

        private void Bitenumeration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var selectedItems = Selections.ToList().Where(x => x.IsChecked).ToArray();
            StrCheckedItems = DisplayFormat(selectedItems);
        }
    }
}
