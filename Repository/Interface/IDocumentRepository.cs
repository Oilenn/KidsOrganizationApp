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

        Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Document>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Document document, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Document document, CancellationToken cancellationToken = default);
    }
}
