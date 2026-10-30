using KidsOrganizationApp.Domain;

namespace KidsOrganizationApp.Repository.Interface
{
    public interface IParentRepository
    {
        Parent GetById(Guid id);
        List<Parent> GetAll();
        void Add(Parent parent);
        void Remove(Guid id);
        void Update(Parent parent);
        List<Parent> GetByName(string name);
        List<Parent> GetBySurname(string surname);
        List<Parent> GetByPatronymic(string patronymic);

        Task<Parent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Parent>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Parent>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<Parent>> GetBySurnameAsync(string surname, CancellationToken cancellationToken = default);
        Task<List<Parent>> GetByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default);
        Task AddAsync(Parent parent, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Parent parent, CancellationToken cancellationToken = default);
    }
}
