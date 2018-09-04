namespace WpfControls
{
    public class Enumeration : BaseViewModel, IEnumeration
    {
        private string label;

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        private string id;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
    }
}
