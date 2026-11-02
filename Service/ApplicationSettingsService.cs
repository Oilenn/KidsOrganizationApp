using System.IO;
using System.Text.Json;

namespace KidsOrganizationApp.Service;

public interface IApplicationSettingsService
{
    string DocumentsDirectory { get; }
    bool IsDarkTheme { get; }
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task SetDocumentsDirectoryAsync(string directory, CancellationToken cancellationToken = default);
    Task SetDarkThemeAsync(bool isDarkTheme, CancellationToken cancellationToken = default);
}

public class ApplicationSettingsService : IApplicationSettingsService
{
    private readonly string _settingsPath;
    private readonly string _directory;
    private readonly SemaphoreSlim _saveLock = new(1, 1);
    private SettingsData _data = new();

    public ApplicationSettingsService()
    {
        _directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KidsOrganizationApp");
        _settingsPath = Path.Combine(_directory, "settings.json");
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_directory);
        if (File.Exists(_settingsPath))
        {
            var json = await File.ReadAllTextAsync(_settingsPath, cancellationToken);
            _data = JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
        }
        else
        {
            _data = new SettingsData();
        }
        if (string.IsNullOrWhiteSpace(_data.DocumentsDirectory))
            _data.DocumentsDirectory = Path.Combine(_directory, "Документы");
        Directory.CreateDirectory(_data.DocumentsDirectory);
        await SaveAsync(cancellationToken);
    }

    public string DocumentsDirectory => _data.DocumentsDirectory;
    public bool IsDarkTheme => _data.IsDarkTheme;

    public async Task SetDocumentsDirectoryAsync(string directory, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(directory);
        _data.DocumentsDirectory = directory;
        await SaveAsync(cancellationToken);
    }

    public async Task SetDarkThemeAsync(bool isDarkTheme, CancellationToken cancellationToken = default)
    {
        _data.IsDarkTheme = isDarkTheme;
        await SaveAsync(cancellationToken);
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _saveLock.WaitAsync(cancellationToken);
        try
        {
            await File.WriteAllTextAsync(_settingsPath, JsonSerializer.Serialize(_data), cancellationToken);
        }
        finally
        {
            _saveLock.Release();
        }
    }

    private sealed class SettingsData
    {
        public string DocumentsDirectory { get; set; } = string.Empty;
        public bool IsDarkTheme { get; set; }
    }
}
