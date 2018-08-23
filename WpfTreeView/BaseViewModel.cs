using System;
using System.ComponentModel;

namespace WpfTreeView
{
    public class BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}