using KidsOrganizationApp.UI.ViewModels;
using System.Windows;

namespace KidsOrganizationApp.UI.View
{
    public partial class ParentChildView : Window
    {


        public ParentChildView()
        {
            InitializeComponent();
        }

        public void SetViewModel(MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
        }
    }
}
