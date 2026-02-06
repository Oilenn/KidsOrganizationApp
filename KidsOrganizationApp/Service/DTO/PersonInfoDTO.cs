using System;

namespace KidsOrganizationApp.Service.DTO
{
    public class PersonInfoDTO
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }
        public ContactDTO? Contact { get; set; }

        public Guid PassportId { get; set; }
        public DocumentDTO? Passport { get; set; }

        public Guid SNILSId { get; set; }
        public DocumentDTO? SNILS { get; set; }

        public Guid DiagnosisFileId { get; set; }
        public DocumentDTO? DiagnosisFile { get; set; }

        public string MembershipStatus { get; set; } = string.Empty;
    }
}
