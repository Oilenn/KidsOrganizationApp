using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddAsync(Parent parent, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            _context.Add(parent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<Parent> GetAll()
        {
            return _context.Parents
                .Include(parent => parent.Documents)
                .ToList();
        }

        public async Task<List<Parent>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Parents
                .Include(parent => parent.Documents)
                .ToListAsync(cancellationToken);
        }

        public Parent GetById(Guid id)
        {
            return _context.Parents
                .Include(parent => parent.Documents)
                .FirstOrDefault(parent => parent.Id == id);
        }

        public async Task<Parent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Parents
                .Include(parent => parent.Documents)
                .FirstOrDefaultAsync(parent => parent.Id == id, cancellationToken);
        }

        public List<Parent> GetByName(string name)
        {
            return _context.Parents
                .Where(c => c.FullName.Name == name)
                .ToList();
        }

        public async Task<List<Parent>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Parents
                .Where(c => c.FullName.Name == name)
                .ToListAsync(cancellationToken);
        }

        public List<Parent> GetByPatronymic(string patronymic)
        {
            return _context.Parents
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToList();
        }

        public async Task<List<Parent>> GetByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Parents
                .Where(c => c.FullName.Patronymic == patronymic)
                .ToListAsync(cancellationToken);
        }

        public List<Parent> GetBySurname(string surname)
        {
            return _context.Parents
                .Where(c => c.FullName.Surname == surname)
                .ToList();
        }

        public async Task<List<Parent>> GetBySurnameAsync(string surname, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            return await _context.Parents
                .Where(c => c.FullName.Surname == surname)
                .ToListAsync(cancellationToken);
        }

        public void Remove(Guid id)
        {
            var parent = GetById(id);
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Parents.Remove(parent);
            _context.SaveChanges();
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var parent = await GetByIdAsync(id, cancellationToken);
            using var operation = await _context.EnterOperationAsync(cancellationToken);
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Parent parent)
        {
            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Update(parent);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Parent parent, CancellationToken cancellationToken = default)
        {
            using var operation = await _context.EnterOperationAsync(cancellationToken);

            if (parent == null)
                throw new NullReferenceException(nameof(parent));

            _context.Update(parent);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
