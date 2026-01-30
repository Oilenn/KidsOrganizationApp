using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service
{
    /// <summary>
    /// Main service as DI container to get depencies in 
    /// </summary>
    public class DIService
    {
        public DIService()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();

            services.AddScoped<ChildService>();

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}

