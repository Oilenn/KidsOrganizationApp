using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;

namespace KidsOrganizationApp.Service.Mapper
{
    public class ContactMapper : IMapper<ContactDTO, Contact>
    {
        public ContactDTO ToDTO(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                MobileNumber = contact.MobileNumber,
                LivingPlace = contact.LivingPlace
            };
        }

        public List<ContactDTO> ToDTO(List<Contact> contacts)
        {
            var dto = new List<ContactDTO>();

            foreach (var contact in contacts)
                dto.Add(ToDTO(contact));

            return dto;
        }

        public Contact ToNewDomain(ContactDTO dto)
        {
            return new Contact(
                dto.MobileNumber,
                dto.LivingPlace
            );
        }
    }
}
