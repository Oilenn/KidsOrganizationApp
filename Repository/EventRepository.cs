using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace KidsOrganizationApp.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Event eventEntity)
        {
            _context.Events.Add(eventEntity);
            _context.SaveChanges();
        }

        public async Task AddAsync(Event eventEntity, CancellationToken cancellationToken = default)
        {
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<Event> GetAll()
        {
            return _context.Events
                .Include(e => e.Documents)
                .ToList();
        }

        public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Events
                .Include(e => e.Documents)
                .ToListAsync(cancellationToken);
        }

        public Event GetById(Guid id)
        {
            return _context.Events
                .Include(e => e.Documents)
                .FirstOrDefault(e => e.Id == id);
        }

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Events
                .Include(e => e.Documents)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public void Update(Event eventEntity)
        {
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Update(eventEntity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Event eventEntity, CancellationToken cancellationToken = default)
        {
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Remove(Guid id)
        {
            var eventEntity = GetById(id);
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Remove(eventEntity);
            _context.SaveChanges();
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var eventEntity = await GetByIdAsync(id, cancellationToken);
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
