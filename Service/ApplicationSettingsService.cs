using System.IO;
using System.Text.Json;

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
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KidsOrganizationApp");
        Directory.CreateDirectory(directory);
        _settingsPath = Path.Combine(directory, "settings.json");
        _data = File.Exists(_settingsPath) ? JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(_settingsPath)) ?? new SettingsData() : new SettingsData();
        if (string.IsNullOrWhiteSpace(_data.DocumentsDirectory)) _data.DocumentsDirectory = Path.Combine(directory, "Документы");
        Directory.CreateDirectory(_data.DocumentsDirectory); Save();
    }
    public string DocumentsDirectory => _data.DocumentsDirectory;
    public bool IsDarkTheme => _data.IsDarkTheme;
    public void SetDocumentsDirectory(string directory) { Directory.CreateDirectory(directory); _data.DocumentsDirectory = directory; Save(); }
    public void SetDarkTheme(bool isDarkTheme) { _data.IsDarkTheme = isDarkTheme; Save(); }
    private void Save() => File.WriteAllText(_settingsPath, JsonSerializer.Serialize(_data));
    private sealed class SettingsData { public string DocumentsDirectory { get; set; } = string.Empty; public bool IsDarkTheme { get; set; } }
}
