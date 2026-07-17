using System.Windows;
using System.Windows.Media;

namespace KidsOrganizationApp.Service;

public interface IThemeService { void Apply(bool isDarkTheme); }
public class ThemeService : IThemeService
{
    public void Apply(bool isDarkTheme)
    {
        Application.Current.Resources["AppBackground"] = Brush(isDarkTheme ? "#111827" : "#F3F4F6");
        Application.Current.Resources["ContentBackground"] = Brush(isDarkTheme ? "#1F2937" : "#FFFFFF");
        Application.Current.Resources["PrimaryText"] = Brush(isDarkTheme ? "#F9FAFB" : "#172033");
        Application.Current.Resources["SecondaryText"] = Brush(isDarkTheme ? "#D1D5DB" : "#5B667A");
        Application.Current.Resources["ControlBorder"] = Brush(isDarkTheme ? "#4B5563" : "#C9D2E1");
        Application.Current.Resources["SuccessText"] = Brush(isDarkTheme ? "#6CE9A6" : "#027A48");
    }

    private static SolidColorBrush Brush(string color) =>
        new((Color)ColorConverter.ConvertFromString(color)!);
}
