using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository
{
    public interface IParentRepository
    {
        Parent GetById(Guid id);
        List<Parent> GetAll();
        void Add(Parent parent);
        void Remove(Parent parent);
        void Update(Parent parent);
    }
}
