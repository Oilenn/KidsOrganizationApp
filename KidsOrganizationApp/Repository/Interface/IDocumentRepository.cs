using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IDocumentRepository
    {
        Document GetById(Guid id);
        List<Document> GetAll();
        void Add(Document document);
        void Remove(Document document);
        void Update(Document document);
    }
}
