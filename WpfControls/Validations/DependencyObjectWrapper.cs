using System.Windows;

namespace WpfControls
{
    public class DependencyObjectWrapper : DependencyObject
    {
        public static readonly DependencyProperty MaxLengthProperty =
           DependencyProperty.Register("MaxLength", typeof(int),
           typeof(DependencyObjectWrapper), new FrameworkPropertyMetadata(int.MaxValue));

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
    }
}
