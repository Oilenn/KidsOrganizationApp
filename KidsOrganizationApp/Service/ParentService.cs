using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;

namespace KidsOrganizationApp.Service
{
    public interface IParentService
    {
        ParentDTO AddParent(ParentDTO dto);
        void UpdateParent(ParentDTO dto);
        void DeleteParent(ParentDTO dto);

        ParentDTO GetParentById(Guid id);
        List<ParentDTO> GetAllParents();

        List<ParentDTO> GetParentsByName(string name);
        List<ParentDTO> GetParentsBySurname(string surname);
        List<ParentDTO> GetParentsByPatronymic(string patronymic);
    }
    public class ParentService : IParentService
    {
        private readonly IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }

        public ParentDTO AddParent(ParentDTO dto)
        {
            Parent parent = ConvertToNewDomain(dto);
            _parentRepository.Add(parent);

            return ConvertToDTO(parent);
        }

        public void UpdateParent(ParentDTO dto)
        {
            Parent parent = ConvertToDomain(dto);
            _parentRepository.Update(parent);
        }

        public void DeleteParent(ParentDTO dto)
        {
            Parent parent = ConvertToDomain(dto);
            _parentRepository.Remove(parent);
        }

        public ParentDTO GetParentById(Guid id)
        {
            Parent parent = _parentRepository.GetById(id);
            return ConvertToDTO(parent);
        }

        public List<ParentDTO> GetAllParents()
        {
            return ConvertToDTO(_parentRepository.GetAll());
        }

        public List<ParentDTO> GetParentsByName(string name)
        {
            return ConvertToDTO(_parentRepository.GetByName(name));
        }

        public List<ParentDTO> GetParentsBySurname(string surname)
        {
            return ConvertToDTO(_parentRepository.GetBySurname(surname));
        }

        public List<ParentDTO> GetParentsByPatronymic(string patronymic)
        {
            return ConvertToDTO(_parentRepository.GetByPatronymic(patronymic));
        }

        private Parent ConvertToNewDomain(ParentDTO dto)
        {
            return new Parent(dto.Name, dto.Surname,dto.Patronymic,dto.DateBirth);
        }

        private Parent ConvertToDomain(ParentDTO dto)
        {
            return _parentRepository.GetById(dto.Id);
        }

        private ParentDTO ConvertToDTO(Parent parent)
        {
            return new ParentDTO
            {
                Id = parent.Id,
                Name = parent.Name,
                Surname = parent.Surname,
                Patronymic = parent.Patronymic,
                DateBirth = parent.DateBirth,
                Children = parent.Children
                    .Select(c => new ChildDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Surname = c.Surname,
                        Patronymic = c.Patronymic,
                        DateBirth = c.DateBirth
                    })
                    .ToList()
            };
        }

        private List<ParentDTO> ConvertToDTO(List<Parent> parents)
        {
            return parents.Select(ConvertToDTO).ToList();
        }
    }
}
