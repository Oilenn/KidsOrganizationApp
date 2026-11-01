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

        public async Task AddAsync(Child child, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            _context.Children.Add(child);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<Child> GetAll()
        {
            return _context.Children
                .Include(child => child.Parents)
                .Include(child => child.Documents)
                .ToList();
        }

        public async Task<List<Child>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Children
                .Include(child => child.Parents)
                .Include(child => child.Documents)
                .ToListAsync(cancellationToken);
        }

        public Child GetById(Guid id)
        {
            return _context.Children
                .Include(child => child.Parents)
                .Include(child => child.Documents)
                .FirstOrDefault(child => child.Id == id);
        }

        public async Task<Child?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Children
                .Include(child => child.Parents)
                .Include(child => child.Documents)
                .FirstOrDefaultAsync(child => child.Id == id, cancellationToken);
        }

        public List<Child> GetByName(string name)
        {
            return _context.Children
                .Where(c => c.FullName.Name == name)
                .ToList();
        }

        public async Task<List<Child>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Children
                .Where(c => c.FullName.Name == name)
                .ToListAsync(cancellationToken);
        }

        public List<Child> GetBySurname(string surname)
        {
            return _context.Children
                .Where(c => c.FullName.Surname == surname)
                .ToList();
        }

        public async Task<List<Child>> GetBySurnameAsync(string surname, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Children
                .Where(c => c.FullName.Surname == surname)
                .ToListAsync(cancellationToken);
        }

        public List<Child> GetByPatronymic(string patronymic)
        {
            return _context.Children
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToList();
        }

        public async Task<List<Child>> GetByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Children
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToListAsync(cancellationToken);
        }

        public void Remove(Guid id)
        {
            var child = GetById(id);
            if (child == null)
                throw new NullReferenceException(nameof(id));

            _context.Remove(child);
            _context.SaveChanges();
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var child = await GetByIdAsync(id, cancellationToken);
            using var operation = await _context.EnterOperationAsync(cancellationToken);
            if (child == null)
                throw new NullReferenceException(nameof(id));

            _context.Remove(child);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Child child)
        {
            if (child == null)
                throw new NullReferenceException(nameof(child));

            _context.Children.Update(child);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Child child, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            if (child == null)
                throw new NullReferenceException(nameof(child));

            _context.Children.Update(child);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
