using System.Collections.ObjectModel;

namespace WpfControls.ViewModels
{
    public class TreeViewModel : UiModel
    {
        public ObservableCollection<TreeNode> TreeNodes { get; set; }
    }
}
