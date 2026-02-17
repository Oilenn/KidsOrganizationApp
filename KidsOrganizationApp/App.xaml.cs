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
using KidsOrganizationApp.Service.Mapper;

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

            // Репозитории
            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<IParentRepository, ParentRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            // Сервисы
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IChildService, ChildService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<WindowService>();

            // Мапперы
            services.AddScoped<ChildMapper>();
            services.AddScoped<DocumentMapper>();
            services.AddScoped<EventMapper>();
            services.AddScoped<ParentMapper>();

            // ViewModel
            services.AddTransient<MainViewModel>();
            services.AddTransient<ParentViewModel>();
            services.AddTransient<ParentChildView>();

            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Hi!");

            //var window = serviceProvider.GetRequiredService<WindowService>();
            //window.ShowParentChildWindow();

            var child = serviceProvider.GetRequiredService<IChildService>();
            var parents = serviceProvider.GetRequiredService<IParentService>();

            var dto = child.AddChild(new Service.DTO.ChildDTO("Настя", "Копшева", "Олеговна",
                "88005553535", "Саратов", DateTime.MinValue));
            var parentdto = parents.Add(new Service.DTO.ParentDTO("Олег", "Копшев", "Анатольевич",
                "88005553535", "Саратов", DateTime.MinValue));


            Console.WriteLine(dto.Name);
            Console.WriteLine(dto.DateBirth);
            Console.WriteLine(child.GetChildById(dto.Id).Name);
            Console.WriteLine(child.GetAllChildren().ToList().Count);
            child.AddParent(parentdto, dto);
            Console.WriteLine(child.GetParents(dto)[0].Name);
        }
    }
}
