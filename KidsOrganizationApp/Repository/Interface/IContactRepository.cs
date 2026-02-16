using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IContactRepository
    {
        Contact GetById(Guid id);
        List<Contact> GetAll();
        void Add(Contact contact);
        void Remove(Contact contact);
        void Update(Contact contact);
    }
}
