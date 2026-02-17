using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public List<Child> GetByName(string name)
        {
            return _context.Children
                .Where(c => c.FullName.Name == name)
                .ToList();
        }
        public List<Child> GetBySurname(string surname)
        {
            return _context.Children
                .Where(c => c.FullName.Surname == surname)
                .ToList();
        }

        public List<Child> GetByPatronymic(string patronymic)
        {
            return _context.Children
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToList();
        }

        public void Remove(Guid id)
        {
            var child = GetById(id);
            if (child == null)
                throw new NullReferenceException(nameof(id));

            _context.Remove(child.Id);
            _context.SaveChanges();
        }

        public void Update(Child child)
        {
            if(child == null)
                throw new NullReferenceException(nameof(child));

            _context.Children.Update(child);
            _context.SaveChanges();
        }
    }
}
