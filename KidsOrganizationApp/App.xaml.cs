using KidsOrganizationApp.Service;
using KidsOrganizationApp.UI.View;
using KidsOrganizationApp.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
﻿using KidsOrganizationApp.Service;
using KidsOrganizationApp.UI.View;
using KidsOrganizationApp.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;

namespace KidsOrganizationApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();

            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IChildService, ChildService>();
            services.AddScoped<WindowService>();

            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<ParentViewModel>();

            services.AddTransient<ParentChildView>();


            var serviceProvider = services.BuildServiceProvider();

            var window = serviceProvider.GetRequiredService<WindowService>();
            window.ShowParentChildWindow();
        }
    }

}
