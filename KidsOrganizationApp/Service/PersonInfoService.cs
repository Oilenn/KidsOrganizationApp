using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;

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

        public PersonInfoService(IPersonInfoRepository personInfoRepository)
        {
            _personInfoRepository = personInfoRepository;
        }

        public PersonInfoDTO Add(PersonInfoDTO dto)
        {
            var person = new PersonInfo
            {
                ContactId = dto.ContactId,
                PassportId = dto.PassportId,
                SNILSId = dto.SNILSId,
                DiagnosisFileId = dto.DiagnosisFileId,
                MembershipStatus = dto.MembershipStatus
            };

            _personInfoRepository.Add(person);
            return ConvertToDTO(person);
        }

        public List<PersonInfoDTO> GetAll()
        {
            return _personInfoRepository.GetAll()
                .Select(ConvertToDTO)
                .ToList();
        }

        public PersonInfoDTO GetById(Guid id)
        {
            var person = _personInfoRepository.GetById(id);
            return ConvertToDTO(person);
        }

        public void Update(PersonInfoDTO dto)
        {
            var person = _personInfoRepository.GetById(dto.Id);

            person.ContactId = dto.ContactId;
            person.PassportId = dto.PassportId;
            person.SNILSId = dto.SNILSId;
            person.DiagnosisFileId = dto.DiagnosisFileId;
            person.MembershipStatus = dto.MembershipStatus;

            _personInfoRepository.Update(person);
        }

        public void Delete(PersonInfoDTO dto)
        {
            var person = _personInfoRepository.GetById(dto.Id);
            _personInfoRepository.Remove(person);
        }

        private PersonInfoDTO ConvertToDTO(PersonInfo person)
        {
            return new PersonInfoDTO
            {
                Id = person.Id,
                ContactId = person.ContactId,
                Contact = person.Contact != null ? new ContactDTO
                {
                    Id = person.Contact.Id,
                    MobileNumber = person.Contact.MobileNumber,
                    LivingPlace = person.Contact.LivingPlace
                } : null,

                PassportId = person.PassportId,
                Passport = person.Passport != null ? new DocumentDTO
                {
                    Id = person.Passport.Id,
                    DocumentType = person.Passport.DocumentType,
                    Path = person.Passport.Path
                } : null,

                SNILSId = person.SNILSId,
                SNILS = person.SNILS != null ? new DocumentDTO
                {
                    Id = person.SNILS.Id,
                    DocumentType = person.SNILS.DocumentType,
                    Path = person.SNILS.Path
                } : null,

                DiagnosisFileId = person.DiagnosisFileId,
                DiagnosisFile = person.DiagnosisFile != null ? new DocumentDTO
                {
                    Id = person.DiagnosisFile.Id,
                    DocumentType = person.DiagnosisFile.DocumentType,
                    Path = person.DiagnosisFile.Path
                } : null,

                MembershipStatus = person.MembershipStatus
            };
        }
    }
}