using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service;
using Microsoft.Win32;
using System.Windows;
using KidsOrganizationApp.UI.View;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class MainViewModel : BaseViewModel
{
    private readonly FamilyViewModel _familyViewModel;
    private readonly DocumentsViewModel _documentsViewModel;
    private readonly EventsViewModel _eventsViewModel;
    private readonly AddFamilyViewModel _addFamilyViewModel;
    private readonly IExportService _exportService;
    private readonly IApplicationSettingsService _settings;
    private readonly SettingsViewModel _settingsViewModel;
    private object _currentViewModel;
    public object CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand OpenFamilyCommand { get; }
    public ICommand OpenDocsCommand { get; }
    public ICommand OpenEventsCommand { get; }
    public ICommand ExportDatabaseCommand { get; }
    public ICommand SelectDocumentsFolderCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand OpenAboutCommand { get; }

    public MainViewModel(
        FamilyViewModel familyViewModel,
        DocumentsViewModel documentsViewModel,
        EventsViewModel eventsViewModel,
        AddFamilyViewModel addFamilyViewModel,
        IExportService exportService,
        IApplicationSettingsService settings,
        SettingsViewModel settingsViewModel)
    {
        _familyViewModel = familyViewModel;
        _documentsViewModel = documentsViewModel;
        _eventsViewModel = eventsViewModel;
        _addFamilyViewModel = addFamilyViewModel;
        _exportService = exportService;
        _settings = settings;
        _settingsViewModel = settingsViewModel;
        _currentViewModel = familyViewModel;
        OpenFamilyCommand = new AsyncRelayCommand(async () =>
        {
            await _familyViewModel.RefreshAsync();
            CurrentViewModel = _familyViewModel;
        });
        OpenDocsCommand = new RelayCommand(() => CurrentViewModel = _documentsViewModel);
        OpenEventsCommand = new AsyncRelayCommand(async () =>
        {
            await _eventsViewModel.RefreshAsync();
            CurrentViewModel = _eventsViewModel;
        });
        ExportDatabaseCommand = new AsyncRelayCommand(ExportDatabaseAsync);
        SelectDocumentsFolderCommand = new AsyncRelayCommand(SelectDocumentsFolderAsync);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        OpenAboutCommand = new RelayCommand(OpenAbout);
        _familyViewModel.AddChildRequested += OpenAddChild;
        _familyViewModel.AddParentRequested += OpenAddParent;
        _familyViewModel.DocumentAddRequested += OpenDocumentsForOwnerAsync;
        _eventsViewModel.DocumentAddRequested += OpenDocumentsForOwnerAsync;
        _addFamilyViewModel.Saved += ReturnToFamilyAsync;
        _addFamilyViewModel.Cancelled += ReturnToFamilyAsync;
    }

    public async Task InitializeAsync()
    {
        await _familyViewModel.RefreshAsync();
        await _documentsViewModel.InitializeAsync();
        await _eventsViewModel.RefreshAsync();
    }

    private void OpenAddChild(ParentDTO parent)
    {
        _addFamilyViewModel.PrepareForChild(parent);
        CurrentViewModel = _addFamilyViewModel;
    }

    private void OpenAddParent()
    {
        _addFamilyViewModel.PrepareForParent();
        CurrentViewModel = _addFamilyViewModel;
    }

    private async Task ReturnToFamilyAsync()
    {
        await _familyViewModel.RefreshAsync();
        CurrentViewModel = _familyViewModel;
    }

    private async Task OpenDocumentsForOwnerAsync(DocumentOwnerType ownerType, Guid ownerId)
    {
        await _documentsViewModel.PrepareForOwnerAsync(ownerType, ownerId);
        CurrentViewModel = _documentsViewModel;
    }

    private async Task ExportDatabaseAsync()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Сохранить архив базы данных",
            Filter = "ZIP-архив (*.zip)|*.zip",
            FileName = $"Выгрузка_организации_{DateTime.Now:yyyy-MM-dd}.zip"
        };
        if (dialog.ShowDialog() != true)
            return;
        try
        {
            await _exportService.ExportArchiveAsync(dialog.FileName);
            MessageBox.Show("Архив успешно создан.", "Выгрузка базы данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Не удалось создать архив", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SelectDocumentsFolderAsync()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Выберите папку для хранения документов",
            InitialDirectory = _settings.DocumentsDirectory
        };
        if (dialog.ShowDialog() == true)
            await _settings.SetDocumentsDirectoryAsync(dialog.FolderName);
    }

    private void OpenSettings()
    {
        var window = new SettingsWindow { DataContext = _settingsViewModel, Owner = Application.Current.MainWindow };
        void CloseWindow() => window.Close();
        _settingsViewModel.Saved += CloseWindow;
        window.Closed += (_, _) => _settingsViewModel.Saved -= CloseWindow;
        window.ShowDialog();
    }

    private static void OpenAbout() => MessageBox.Show(
        "Организация семей, имеющих детей инвалидов\nВерсия: 1.0.0\n\nGitHub: https://github.com/Oilenn/KidsOrganizationApp",
        "О программе",
        MessageBoxButton.OK,
        MessageBoxImage.Information);
}
