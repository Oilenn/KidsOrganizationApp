using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;

namespace KidsOrganizationApp.Service.Mapper
{
    public class PersonInfoMapper : IMapper<PersonInfoDTO, PersonInfo>
    {
        private readonly ContactMapper _contactMapper;
        private readonly DocumentMapper _documentMapper;

        public PersonInfoMapper(ContactMapper contactMapper, DocumentMapper documentMapper)
        {
            _contactMapper = contactMapper;
            _documentMapper = documentMapper;
        }

        public PersonInfoDTO ToDTO(PersonInfo personInfo)
        {
            return new PersonInfoDTO
            {
                Id = personInfo.Id,

                ContactId = personInfo.Contact?.Id ?? Guid.Empty,
                Contact = personInfo.Contact != null
                    ? _contactMapper.ToDTO(personInfo.Contact)
                    : null,

                PassportId = personInfo.Passport?.Id ?? Guid.Empty,
                Passport = personInfo.Passport != null
                    ? _documentMapper.ToDTO(personInfo.Passport)
                    : null,

                SNILSId = personInfo.SNILS?.Id ?? Guid.Empty,
                SNILS = personInfo.SNILS != null
                    ? _documentMapper.ToDTO(personInfo.SNILS)
                    : null,

                DiagnosisFileId = personInfo.DiagnosisFile?.Id ?? Guid.Empty,
                DiagnosisFile = personInfo.DiagnosisFile != null
                    ? _documentMapper.ToDTO(personInfo.DiagnosisFile)
                    : null,

                MembershipStatus = personInfo.MembershipStatus.ToString()
            };
        }

        public List<PersonInfoDTO> ToDTO(List<PersonInfo> persons)
        {
            var dto = new List<PersonInfoDTO>();

            foreach (var person in persons)
                dto.Add(ToDTO(person));

            return dto;
        }

        public PersonInfo ToNewDomain(PersonInfoDTO dto)
        {
            var contact = dto.Contact != null
                ? _contactMapper.ToNewDomain(dto.Contact)
                : null;

            var passport = dto.Passport != null
                ? _documentMapper.ToNewDomain(dto.Passport)
                : null;

            var snils = dto.SNILS != null
                ? _documentMapper.ToNewDomain(dto.SNILS)
                : null;

            var diagnosis = dto.DiagnosisFile != null
                ? _documentMapper.ToNewDomain(dto.DiagnosisFile)
                : null;

            var status = Enum.TryParse<MembershipStatus>(dto.MembershipStatus, out var parsedStatus)
                ? parsedStatus
                : MembershipStatus.Active;

            return new PersonInfo(
                contact!,
                passport!,
                snils!,
                diagnosis!,
                status
            );
        }
    }
}
