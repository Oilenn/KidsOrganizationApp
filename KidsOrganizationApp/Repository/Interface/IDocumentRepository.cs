using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IDocumentRepository
    {
        Document GetById(Guid id);
        List<Document> GetAll();
        void Add(Document document);
        void Remove(Guid id);
        void Update(Document document);
    }
}
