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
            var childDTO = new ChildDTO
            (
                child.Id,
                child.FullName.Name,
                child.FullName.Surname,
                child.FullName.Patronymic,
                child.Contact.MobileNumber,
                child.Contact.LivingPlace,
                child.DateBirth,
                child.Contact.Email
            );

            return childDTO;
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
            var child = new Child(
                new FullName(dto.Name, dto.Surname, dto.Patronymic),
                new Contact(dto.MobileNumber, dto.LivingPlace, dto.Email),
                dto.DateBirth,
                new List<Parent>(),
                new List<Document>());

            return child;
        }

        public void UpdateDomain(Child domain, ChildDTO dto)
        {
            domain.Contact = new Contact(dto.MobileNumber, dto.LivingPlace, dto.Email);
            domain.FullName = new FullName(dto.Name, dto.Surname, dto.Patronymic);

            domain.ChangeDateBirth(dto.DateBirth);
            domain.MembershipStatus = (MembershipStatus) dto.MembershipStatus;
        }
    }
}
    