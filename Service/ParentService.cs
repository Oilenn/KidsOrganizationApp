using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IParentService
    {
        Task<ParentDTO> AddAsync(ParentDTO dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(ParentDTO dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ParentDTO> GetParentByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ParentDTO>> GetAllParentsAsync(CancellationToken cancellationToken = default);

        Task<List<ParentDTO>> GetParentsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<ParentDTO>> GetParentsBySurnameAsync(string surname, CancellationToken cancellationToken = default);
        Task<List<ParentDTO>> GetParentsByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default);
    }
    public class ParentService : IParentService
    {
        private readonly IParentRepository _parentRepository;
        private readonly IMapper<ParentDTO, Parent> _mapper;

        public ParentService(IParentRepository parentRepository, ParentMapper mapper)
        {
            _parentRepository = parentRepository;
            _mapper = mapper;
        }

        public async Task<ParentDTO> AddAsync(ParentDTO dto, CancellationToken cancellationToken = default)
        {
            Parent parent = _mapper.ToNewDomain(dto);
            await _parentRepository.AddAsync(parent, cancellationToken);

            return _mapper.ToDTO(parent);
        }

        public async Task UpdateAsync(ParentDTO dto, CancellationToken cancellationToken = default)
        {
            var parent = await _parentRepository.GetByIdAsync(dto.Id, cancellationToken);
            _mapper.UpdateDomain(parent!, dto);

            await _parentRepository.UpdateAsync(parent!, cancellationToken);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _parentRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<ParentDTO> GetParentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var parent = await _parentRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.ToDTO(parent!);
        }

        public async Task<List<ParentDTO>> GetAllParentsAsync(CancellationToken cancellationToken = default)
        {
            var parents = await _parentRepository.GetAllAsync(cancellationToken);
            return _mapper.ToDTO(parents);
        }

        public async Task<List<ParentDTO>> GetParentsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var parents = await _parentRepository.GetByNameAsync(name, cancellationToken);
            return _mapper.ToDTO(parents);
        }

        public async Task<List<ParentDTO>> GetParentsBySurnameAsync(
            string surname,
            CancellationToken cancellationToken = default)
        {
            var parents = await _parentRepository.GetBySurnameAsync(surname, cancellationToken);
            return _mapper.ToDTO(parents);
        }

        public async Task<List<ParentDTO>> GetParentsByPatronymicAsync(
            string patronymic,
            CancellationToken cancellationToken = default)
        {
            var parents = await _parentRepository.GetByPatronymicAsync(patronymic, cancellationToken);
            return _mapper.ToDTO(parents);
        }
    }
}
