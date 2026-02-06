using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository
{
    public interface IEventRepository
    {
        Event GetById(Guid id);
        List<Event> GetAll();
        void Add(Event newEvent);
        void Remove(Event eventToDelete);
        void Update(Event eventToUpdate);
    }
}
