using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace KidsOrganizationApp.UI.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Provider.GetRequiredService<MainViewModel>();
    }
}
