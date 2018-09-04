using System.Collections.ObjectModel;

namespace WpfControls
{
    public class TreeNode : BaseViewModel, ITreeNode
    {
        public string Label { get; set; }
        public string Id { get; set; }

        public ObservableCollection<TreeNode> TreeNodes { get; set; }
    }
}
