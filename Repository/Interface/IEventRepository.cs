using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IEventRepository
    {
        Event GetById(Guid id);
        List<Event> GetAll();
        void Add(Event newEvent);
        void Remove(Guid id);
        void Update(Event eventToUpdate);

        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Event newEvent, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Event eventToUpdate, CancellationToken cancellationToken = default);
    }
}
