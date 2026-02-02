using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository
{
    public interface IChildRepository
    {
        Child GetById(Guid id);
        List<Child> GetAll();
        void Add(Child child);
        void Remove(Child child);
        void Update(Child child);
    }
}
