using KidsOrganizationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Repository
{
    public class ChildRepository : IChildRepository
    {
        private AppDbContext _context;

        public ChildRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void Add(Child child)
        {
            _context.Children.Add(child);
            _context.SaveChanges();
        }

        public List<Child> GetAll()
        {
            return _context.Children.ToList();
        }

        public Child GetById(Guid id)
        {
            return _context.Children.Find(id);
        }

        public void Remove(Child child)
        {
            _context.Remove(child);
        }

        public void Update(Child child)
        {
            _context.Update(child);
        }
    }
}
