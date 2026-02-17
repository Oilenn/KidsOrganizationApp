using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace KidsOrganizationApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Parent> Parents => Set<Parent>();
        public DbSet<Child> Children => Set<Child>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Event> Events => Set<Event>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=kids_org.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
