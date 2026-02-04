

using KidsOrganizationApp.Domain;
using System.Xml.Linq;

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

        public List<Parent> GetByName(string name)
        {
            return _context.Parents
                .Where(c => c.Name == name)
                .ToList();
        }

        public List<Parent> GetByPatronymic(string patronymic)
        {
            return _context.Parents
                .Where(c => c.Patronymic == patronymic)
                .ToList();
        }

        public List<Parent> GetBySurname(string surname)
        {
            return _context.Parents
                .Where(c => c.Surname == surname)
                .ToList();
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
