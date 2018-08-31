using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfControls.Annotations;

namespace WpfControls
{
    public class GroupBoxControlViewModel : ListViewItemModel
    {
        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        public ObservableCollection<ListViewItemModel> ListViewItemModels { get; set; }
    }
}
