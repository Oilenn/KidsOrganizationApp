using System.ComponentModel;
using System.Windows.Controls;

namespace KidsOrganizationApp.UI.View.UserControls
{
    public partial class MyTextBox : UserControl, INotifyPropertyChanged
    {
        private string _placeHolder;

        public event PropertyChangedEventHandler? PropertyChanged;
        public MyTextBox()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string PlaceHolder 
        { 
            get { return _placeHolder; } 
            set 
            {
                _placeHolder = value;
                OnPropertyChanged(nameof(PlaceHolder));
            } 
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBox.Text))
                tbText.Visibility = System.Windows.Visibility.Visible;
            else
                tbText.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
