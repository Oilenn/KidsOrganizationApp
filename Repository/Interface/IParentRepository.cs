using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IParentRepository
    {
        Parent GetById(Guid id);
        List<Parent> GetAll();
        void Add(Parent parent);
        void Remove(Guid id);
        void Update(Parent parent);
        List<Parent> GetByName(string name);
        List<Parent> GetBySurname(string surname);
        List<Parent> GetByPatronymic(string patronymic);
    }
}
