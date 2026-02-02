

using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository
{
    public class ParentRepository : IParentRepository
    {
        private AppDbContext _context;

        public ParentRepository(AppDbContext appDbContext) 
        {
            _context = appDbContext;
        }
        public void Add(Parent parent)
        {
            _context.Add(parent);
        }

        public List<Parent> GetAll()
        {
            return _context.Parents.ToList();
        }

        public Parent GetById(Guid id)
        {
            return _context.Parents.Find(id);
        }

        public void Remove(Parent parent)
        {
            _context.Remove(parent);
        }

        public void Update(Parent parent)
        {
            _context.Update(parent);
        }
    }
}
