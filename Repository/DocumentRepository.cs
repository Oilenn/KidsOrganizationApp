using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KidsOrganizationApp.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Document document)
        {
            _context.Documents.Add(document);
            _context.SaveChanges();
        }

        public async Task AddAsync(Document document, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            _context.Documents.Add(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<Document> GetAll()
        {
            return _context.Documents.ToList();
        }

        public async Task<List<Document>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Documents.ToListAsync(cancellationToken);
        }

        public Document GetById(Guid id)
        {
            return _context.Documents.Find(id);
        }

        public async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Documents.FindAsync([id], cancellationToken);
        }

        public void Update(Document document)
        {
            if (document == null)
                throw new NullReferenceException(nameof(document));

            _context.Documents.Update(document);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Document document, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            if (document == null)
                throw new NullReferenceException(nameof(document));

            _context.Documents.Update(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Remove(Guid id)
        {
            var doc = GetById(id);
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            _context.Remove(doc);
            _context.SaveChanges();
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var doc = await GetByIdAsync(id, cancellationToken);
            using var operation = await _context.EnterOperationAsync(cancellationToken);
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            _context.Remove(doc);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
