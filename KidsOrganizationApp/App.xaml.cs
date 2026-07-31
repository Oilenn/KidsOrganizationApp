using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace KidsOrganizationApp;

public partial class App : Application
{
    public static IServiceProvider Provider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IChildRepository, ChildRepository>();
        services.AddScoped<IParentRepository, ParentRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IParentService, ParentService>();
        services.AddScoped<IChildService, ChildService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDocumentFileService, DocumentFileService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFamilyService, FamilyService>();
        services.AddScoped<IExportService, ExportService>();
        services.AddSingleton<IApplicationSettingsService, ApplicationSettingsService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddScoped<ChildMapper>();
        services.AddScoped<DocumentMapper>();
        services.AddScoped<EventMapper>();
        services.AddScoped<ParentMapper>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<FamilyViewModel>();
        services.AddTransient<AddFamilyViewModel>();
        services.AddTransient<DocumentsViewModel>();
        services.AddTransient<EventsViewModel>();
        services.AddTransient<SettingsViewModel>();
        Provider = services.BuildServiceProvider();
        Provider.GetRequiredService<IThemeService>().Apply(Provider.GetRequiredService<IApplicationSettingsService>().IsDarkTheme);
        base.OnStartup(e);
    }
}
