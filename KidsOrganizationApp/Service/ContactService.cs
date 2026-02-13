using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

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
        private readonly IMapper<ContactDTO, Contact> _mapper;
        public ContactService(IContactRepository contactRepository, ContactMapper contactMapper)
        {
            _contactRepository = contactRepository;
            _mapper = contactMapper;
        }

        public ContactDTO Add(ContactDTO dto)
        {
            _contactRepository.Add(_mapper.ToNewDomain(dto));
            return dto;
        }

        public List<ContactDTO> GetAll()
        {
            return _contactRepository.GetAll()
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public ContactDTO GetById(Guid id)
        {
            return _mapper.ToDTO(_contactRepository.GetById(id));
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
    }
}
