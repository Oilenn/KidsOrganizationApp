using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IPersonInfoService
    {
        PersonInfoDTO Add(PersonInfoDTO dto);
        PersonInfoDTO GetById(Guid id);
        List<PersonInfoDTO> GetAll();
        void Update(PersonInfoDTO dto);
        void Delete(PersonInfoDTO dto);
    }

    public class PersonInfoService : IPersonInfoService
    {
        private readonly IPersonInfoRepository _personInfoRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper<PersonInfoDTO, PersonInfo> _mapper;

        public PersonInfoService(IPersonInfoRepository personInfoRepository,
                                IDocumentRepository documentRepository,
                                IContactRepository contactRepository,
                                PersonInfoMapper mapper)
        {
            _personInfoRepository = personInfoRepository;
            _documentRepository = documentRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public PersonInfoDTO Add(PersonInfoDTO dto)
        {
            var person = _mapper.ToNewDomain(dto);

            _personInfoRepository.Add(person);
            return _mapper.ToDTO(person);
        }

        public List<PersonInfoDTO> GetAll()
        {
            return _personInfoRepository.GetAll()
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public PersonInfoDTO GetById(Guid id)
        {
            var person = _personInfoRepository.GetById(id);
            return _mapper.ToDTO(person);
        }

        public void Update(PersonInfoDTO dto)
        {
            var person = _personInfoRepository.GetById(dto.Id);
            _personInfoRepository.Update(person);
        }

        public void Delete(PersonInfoDTO dto)
        {
            var person = _personInfoRepository.GetById(dto.Id);
            _personInfoRepository.Remove(person);
        }
    }
}