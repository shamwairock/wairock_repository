using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfControls
{
    public class DatePickerControlViewModel : ListViewItemModel
    {
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
    }
}
