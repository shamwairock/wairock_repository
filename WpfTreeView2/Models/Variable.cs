using WpfTreeView2.Interfaces;

namespace WpfTreeView2.Models
{
    public class Variable : IUiElement
    {
        private string _label;
        public string Label
        {
            get { return _label; }

            set
            {
                _label = value;
            }
        }

        private string _toolTip;
        public string ToolTip
        {
            get { return _toolTip; }

            set
            {
                _toolTip = value;
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }

            set
            {
                _value = value;
            }
        }

        private string _nodePath;
        public string NodePath
        {
            get { return _nodePath; }

            set { _nodePath = value; }
        }

        public Variable(string nodePath, string label, string toolTip)
        {
            _nodePath = nodePath;
            _label = label;
            _toolTip = toolTip;
        }
    }
}
