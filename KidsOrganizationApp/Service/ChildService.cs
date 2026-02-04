using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KidsOrganizationApp.Service
{
    public interface IChildService
    {
        ChildDTO AddChild(ChildDTO dto);
        ChildDTO GetChildById(Guid id);

        void DeleteChild(ChildDTO dto);
        void UpdateChild(ChildDTO dto);

        List<ChildDTO> GetAllChildren();
        List<ChildDTO> GetChildrenByName(string name);
        List<ChildDTO> GetChildrenBySurname(string surname);
        List<ChildDTO> GetChildrenByPatronymic(string patronomyc);
    }

    public class ChildService : IChildService
    {
        private IChildRepository _childRepository;

        public ChildService(IChildRepository childRepository) 
        {
            _childRepository = childRepository;
        }

        public ChildDTO AddChild(ChildDTO dto)
        {
            Child child = ConvertToNewDomain(dto);
            _childRepository.Add(child);

            return ConvertToDTO(child);
        }

        public List<ChildDTO> GetAllChildren()
        {
            List<ChildDTO> children = ConvertToDTO(_childRepository.GetAll());

            return children;
        }

        public ChildDTO GetChildById(Guid id)
        {
            return ConvertToDTO(_childRepository.GetById(id));
        }

        public List<ChildDTO> GetChildrenByName(string name)
        {
            return ConvertToDTO(_childRepository.GetByName(name));
        }

        public List<ChildDTO> GetChildrenBySurname(string surname)
        {
            return ConvertToDTO(_childRepository.GetBySurname(surname));
        }

        public List<ChildDTO> GetChildrenByPatronymic(string patronomyc)
        {
            return ConvertToDTO(_childRepository.GetBySurname(patronomyc));
        }

        public void DeleteChild(ChildDTO dto)
        {
            _childRepository.Remove(ConvertToDomain(dto));
        }

        public void UpdateChild(ChildDTO dto)
        {
            _childRepository.Update(ConvertToDomain(dto));
        }

        private List<ChildDTO> ConvertToDTO(List<Child> children)
        {
            List<ChildDTO> dto = new List<ChildDTO>();
            foreach (Child child in children)
            {
                dto.Add(ConvertToDTO(child));
            }

            return dto;
        }

        private ChildDTO ConvertToDTO(Child child)
        {
            return new ChildDTO()
            {
                Id = child.Id,
                Name = child.Name,
                Surname = child.Surname,
                DateBirth = child.DateBirth
            };
        }

        private Child ConvertToDomain(ChildDTO dto)
        {
            return _childRepository.GetById(dto.Id);
        }

        private Child ConvertToNewDomain(ChildDTO dto)
        {
            return new Child(dto.Name,
                dto.Surname,
                dto.Patronymic,
                dto.DateBirth);
        }
    }
}
