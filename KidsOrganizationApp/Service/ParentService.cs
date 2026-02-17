using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IParentService
    {
        ParentDTO Add(ParentDTO dto);
        void Update(ParentDTO dto);
        void Delete(Guid id);

        ParentDTO GetParentById(Guid id);
        List<ParentDTO> GetAllParents();

        List<ParentDTO> GetParentsByName(string name);
        List<ParentDTO> GetParentsBySurname(string surname);
        List<ParentDTO> GetParentsByPatronymic(string patronymic);
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

        public ParentDTO Add(ParentDTO dto)
        {
            Parent parent = _mapper.ToNewDomain(dto);
            _parentRepository.Add(parent);

            return _mapper.ToDTO(parent);
        }

        public void Update(ParentDTO dto)
        {
            var parent = _parentRepository.GetById(dto.Id);
            _mapper.UpdateDomain(parent, dto);

            _parentRepository.Update(parent);
        }

        public void Delete(Guid id)
        {
            _parentRepository.Remove(id);
        }

        public ParentDTO GetParentById(Guid id)
        {
            Parent parent = _parentRepository.GetById(id);
            return _mapper.ToDTO(parent);
        }

        public List<ParentDTO> GetAllParents()
        {
            return _mapper.ToDTO(_parentRepository.GetAll());
        }

        public List<ParentDTO> GetParentsByName(string name)
        {
            return _mapper.ToDTO(_parentRepository.GetByName(name));
        }

        public List<ParentDTO> GetParentsBySurname(string surname)
        {
            return _mapper.ToDTO(_parentRepository.GetBySurname(surname));
        }

        public List<ParentDTO> GetParentsByPatronymic(string patronymic)
        {
            return _mapper.ToDTO(_parentRepository.GetByPatronymic(patronymic));
        }
    }
}
