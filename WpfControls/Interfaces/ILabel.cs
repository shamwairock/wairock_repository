using System.ComponentModel;

namespace WpfControls
{
    public interface ILabel : INotifyPropertyChanged
    {
        string Label { get; set; }
    }
}
