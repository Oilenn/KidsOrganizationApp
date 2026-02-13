using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.Mapper
{
    public class ChildMapper : IMapper<ChildDTO, Child>
    {
        public ChildDTO ToDTO(Child child)
        {
            return new ChildDTO
            {
                Id = child.Id,
                Name = child.Name,
                Surname = child.Surname,
                Patronymic = child.Patronymic,
                DateBirth = child.DateBirth
            };
        }

        public List<ChildDTO> ToDTO(List<Child> children)
        {
            var dto = new List<ChildDTO>();

            foreach (var child in children)
                dto.Add(ToDTO(child));

            return dto;
        }

        public Child ToNewDomain(ChildDTO dto)
        {
            return new Child(
                dto.Name,
                dto.Surname,
                dto.Patronymic,
                dto.DateBirth
            );
        }
    }
}
    