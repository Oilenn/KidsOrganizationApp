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
    }
}
