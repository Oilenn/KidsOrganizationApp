using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.UI.View;
using System.Windows.Input;

public class MainViewModel
{
    private FamilyViewModel _familyViewModel;

    public ICommand OpenFamilyCommand { get; }
    public ICommand OpenDocsCommand { get; }
    public ICommand DownloadDbCommand { get; }

    public MainViewModel(FamilyViewModel familyViewModel)
    {
        _familyViewModel = familyViewModel;

        OpenFamilyCommand = new RelayCommand(OpenFamily);
        OpenDocsCommand = new RelayCommand(OpenDocs);
        DownloadDbCommand = new RelayCommand(DownloadDb);
    }

    private void OpenFamily()
    {
        var window = new FamilyView();
        window.DataContext = _familyViewModel;
        window.Show();
    }

    private void OpenDocs()
    {
        var window = new FamilyView();
        window.DataContext = _familyViewModel;
        window.Show();
    }

    private void DownloadDb()
    {
        var window = new FamilyView();
        window.DataContext = _familyViewModel;
        window.Show();
    }
}