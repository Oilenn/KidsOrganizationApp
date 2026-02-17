using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace KidsOrganizationApp.Service.Mapper
{
    public class ParentMapper : IMapper<ParentDTO, Parent>
    {

        public ParentDTO ToDTO(Parent parent)
        {
            return new ParentDTO(
                    parent.Id,
                    parent.FullName.Name,
                    parent.FullName.Surname,
                    parent.FullName.Patronymic,
                    parent.Contact.MobileNumber,
                    parent.Contact.LivingPlace,
                    parent.DateBirth,
                    parent.Contact.Email
                );
        }

        public List<ParentDTO> ToDTO(List<Parent> parents)
        {
            var dto = new List<ParentDTO>();

            foreach (var parent in parents)
                dto.Add(ToDTO(parent));

            return dto;
        }

        public Parent ToNewDomain(ParentDTO dto)
        {
            var parent = new Parent(
                new FullName(dto.Name, dto.Surname, dto.Patronymic),
                new Contact(dto.MobileNumber, dto.LivingPlace, dto.Email),
                dto.DateBirth,
                new List<Document>());

            return parent;
        }

        public void UpdateDomain(Parent domain, ParentDTO dto)
        {
            domain.Contact = new Contact(dto.MobileNumber, dto.LivingPlace, dto.Email);
            domain.FullName = new FullName(dto.Name, dto.Surname, dto.Patronymic);

            domain.ChangeDateBirth(dto.DateBirth);
            domain.MembershipStatus = (MembershipStatus) dto.MembershipStatus;
        }
    }
}
