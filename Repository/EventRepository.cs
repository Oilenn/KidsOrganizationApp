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

        public List<Event> GetAll()
        {
            return _context.Events
                .Include(e => e.Documents)
                .ToList();
        }

        public Event GetById(Guid id)
        {
            return _context.Events
                    .Include(e => e.Documents)
                    .FirstOrDefault(e => e.Id == id);
        }

        public void Update(Event eventEntity)
        {
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Update(eventEntity);
            _context.SaveChanges();
        }

        public void Remove(Guid id)
        {
            var eventEntity = GetById(id);
            if (eventEntity == null)
                throw new NullReferenceException(nameof(eventEntity));

            _context.Events.Remove(eventEntity);
            _context.SaveChanges();
        }
    }
}
