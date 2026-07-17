using System.Text.Json;
using System.IO;

namespace KidsOrganizationApp.Service;

public interface IApplicationSettingsService
{
    string DocumentsDirectory { get; }
    bool IsDarkTheme { get; }
    void SetDocumentsDirectory(string directory);
    void SetDarkTheme(bool isDarkTheme);
}

public class ApplicationSettingsService : IApplicationSettingsService
{
    private readonly string _settingsPath;
    private SettingsData _data;

    public ApplicationSettingsService()
    {
        var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KidsOrganizationApp");
        Directory.CreateDirectory(appDirectory);
        _settingsPath = Path.Combine(appDirectory, "settings.json");
        _data = Load();
        if (string.IsNullOrWhiteSpace(_data.DocumentsDirectory))
        {
            _data.DocumentsDirectory = Path.Combine(appDirectory, "Документы");
            Save();
        }
        Directory.CreateDirectory(_data.DocumentsDirectory);
    }

    public string DocumentsDirectory => _data.DocumentsDirectory;
    public bool IsDarkTheme => _data.IsDarkTheme;
    public void SetDocumentsDirectory(string directory) { _data.DocumentsDirectory = directory; Directory.CreateDirectory(directory); Save(); }
    public void SetDarkTheme(bool isDarkTheme) { _data.IsDarkTheme = isDarkTheme; Save(); }

    private SettingsData Load()
    {
        if (!File.Exists(_settingsPath)) return new SettingsData();
        return JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(_settingsPath)) ?? new SettingsData();
    }
    private void Save() => File.WriteAllText(_settingsPath, JsonSerializer.Serialize(_data));
    private sealed class SettingsData { public string DocumentsDirectory { get; set; } = string.Empty; public bool IsDarkTheme { get; set; } }
}
