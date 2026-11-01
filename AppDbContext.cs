using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace KidsOrganizationApp
{
    public class AppDbContext : DbContext
    {
        private readonly SemaphoreSlim _operationLock = new(1, 1);

        public DbSet<Parent> Parents => Set<Parent>();
        public DbSet<Child> Children => Set<Child>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Event> Events => Set<Event>();

        internal async Task<IDisposable> EnterOperationAsync(CancellationToken cancellationToken)
        {
            // Все репозитории используют один контекст, поэтому запросы выполняются последовательно.
            await _operationLock.WaitAsync(cancellationToken);
            return new OperationScope(_operationLock);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=kids_org.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        private sealed class OperationScope(SemaphoreSlim operationLock) : IDisposable
        {
            public void Dispose() => operationLock.Release();
        }
    }
}
