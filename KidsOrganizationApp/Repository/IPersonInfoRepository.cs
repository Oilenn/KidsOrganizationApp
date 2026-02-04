using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository
{
    public interface IPersonInfoRepository
    {
        PersonInfo GetById(Guid id);
        List<PersonInfo> GetAll();
        void Add(PersonInfo personInfo);
        void Remove(PersonInfo personInfo);
        void Update(PersonInfo personInfo);
    }
}
