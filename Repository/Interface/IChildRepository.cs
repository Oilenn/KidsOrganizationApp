using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IChildRepository
    {
        Child GetById(Guid id);
        List<Child> GetAll();
        List<Child> GetByName(string name);
        List<Child> GetBySurname(string surname);
        List<Child> GetByPatronymic(string patronymic);
        void Add(Child child);
        void Remove(Guid id);
        void Update(Child child);

        Task<Child?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Child>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Child>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<Child>> GetBySurnameAsync(string surname, CancellationToken cancellationToken = default);
        Task<List<Child>> GetByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default);
        Task AddAsync(Child child, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Child child, CancellationToken cancellationToken = default);
    }
}
