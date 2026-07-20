using System.Windows;
using System.Windows.Media;

namespace KidsOrganizationApp.Service;
public interface IThemeService { void Apply(bool isDarkTheme); }
public class ThemeService : IThemeService
{
    public void Apply(bool dark)
    {
        Application.Current.Resources["AppBackground"] = Brush(dark ? "#111827" : "#F3F4F6");
        Application.Current.Resources["ContentBackground"] = Brush(dark ? "#1F2937" : "#FFFFFF");
        Application.Current.Resources["PrimaryText"] = Brush(dark ? "#F9FAFB" : "#172033");
        Application.Current.Resources["SecondaryText"] = Brush(dark ? "#D1D5DB" : "#5B667A");
        Application.Current.Resources["AccentText"] = Brush(dark ? "#B9D1FF" : "#315EA8");
        Application.Current.Resources["ErrorText"] = Brush(dark ? "#FFB4AB" : "#B42318");
        Application.Current.Resources["ControlBorder"] = Brush(dark ? "#4B5563" : "#C9D2E1");
        Application.Current.Resources["SuccessText"] = Brush(dark ? "#6CE9A6" : "#027A48");
    }
    private static SolidColorBrush Brush(string color) => new((Color)ColorConverter.ConvertFromString(color)!);
}
