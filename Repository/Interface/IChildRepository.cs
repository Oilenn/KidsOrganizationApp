using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IChildRepository
    {
        Child GetById(Guid id);
        List<Child> GetAll();
        List<Child> GetByName(string name);
        List<Child> GetBySurname(string surname);
        List<Child> GetByPatronymic(string patronymic);
        void Add(Child child);
        void Remove(Guid id);
        void Update(Child child);
    }
}
