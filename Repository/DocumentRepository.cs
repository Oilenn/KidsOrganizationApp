using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;

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

        public List<Document> GetAll()
        {
            return _context.Documents.ToList();
        }

        public Document GetById(Guid id)
        {
            return _context.Documents.Find(id);
        }

        public void Update(Document document)
        {
            if (document == null)
                throw new NullReferenceException(nameof(document));

            _context.Documents.Update(document);
            _context.SaveChanges();
        }

        public void Remove(Guid id)
        {
            var doc = GetById(id);
            if (doc == null)
                throw new NullReferenceException(nameof(doc));
            
            _context.Remove(doc.Id);
            _context.SaveChanges();
        }
    }
}
