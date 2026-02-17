

using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
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
            _context.SaveChanges();
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
                .Where(c => c.FullName.Name == name)
                .ToList();
        }

        public List<Parent> GetByPatronymic(string patronymic)
        {
            return _context.Parents
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToList();
        }

        public List<Parent> GetBySurname(string surname)
        {
            return _context.Parents
                .Where(c => c.FullName.Surname == surname)
                .ToList();
        }

        public void Remove(Guid id)
        {
            var parent = GetById(id);
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Parents.Remove(parent);
            _context.SaveChanges();
        }

        public void Update(Parent parent)
        {
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Update(parent);
            _context.SaveChanges();
        }
    }
}
