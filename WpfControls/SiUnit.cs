using System.ComponentModel;
using WpfControls.Annotations;

namespace WpfControls
{
    public class SiUnit : IRelation, INotifyPropertyChanged
    {
        private string label;

        public string Label
        {
            get
            {
                if (label == string.Empty)
                {
                    label = "Please select";
                }
                return label;
            }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
