using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class SettingsViewModel : BaseViewModel
{
    private readonly IApplicationSettingsService _settings;
    private readonly IThemeService _themeService;
    public event Action? Saved;
    private bool _isDarkTheme;
    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            _isDarkTheme = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsLightTheme));
        }
    }

    public bool IsLightTheme
    {
        get => !IsDarkTheme;
        set
        {
            if (value)
                IsDarkTheme = false;
        }
    }

    public ICommand SaveCommand { get; }

    public SettingsViewModel(IApplicationSettingsService settings, IThemeService themeService)
    {
        _settings = settings;
        _themeService = themeService;
        _isDarkTheme = settings.IsDarkTheme;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        var isDarkTheme = IsDarkTheme;
        await _settings.SetDarkThemeAsync(isDarkTheme);
        _themeService.Apply(isDarkTheme);
        Saved?.Invoke();
    }
}
