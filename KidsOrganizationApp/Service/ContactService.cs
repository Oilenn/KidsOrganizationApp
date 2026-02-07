using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;

namespace KidsOrganizationApp.Service
{
    public interface IContactService
    {
        ContactDTO Add(ContactDTO dto);
        ContactDTO GetById(Guid id);
        List<ContactDTO> GetAll();
        void Update(ContactDTO dto);
        void Delete(ContactDTO dto);
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ContactDTO Add(ContactDTO dto)
        {
            var contact = new Contact(dto.MobileNumber, dto.LivingPlace);
            _contactRepository.Add(contact);
            return ConvertToDTO(contact);
        }

        public List<ContactDTO> GetAll()
        {
            return _contactRepository.GetAll()
                .Select(ConvertToDTO)
                .ToList();
        }

        public ContactDTO GetById(Guid id)
        {
            return ConvertToDTO(_contactRepository.GetById(id));
        }

        public void Update(ContactDTO dto)
        {
            var contact = _contactRepository.GetById(dto.Id);
            _contactRepository.Update(contact);
        }

        public void Delete(ContactDTO dto)
        {
            _contactRepository.Remove(_contactRepository.GetById(dto.Id));
        }

        private ContactDTO ConvertToDTO(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                MobileNumber = contact.MobileNumber,
                LivingPlace = contact.LivingPlace
            };
        }
    }
}
