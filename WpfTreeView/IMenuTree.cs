using System.Collections.ObjectModel;

namespace WpfTreeView
{
    public interface IMenuTree
    {
        string Label { get; set; }

        ObservableCollection<IMenuTree> MenuTrees { get; set; }

        ObservableCollection<IParameter>  Parameters { get; set; }
    }
}
