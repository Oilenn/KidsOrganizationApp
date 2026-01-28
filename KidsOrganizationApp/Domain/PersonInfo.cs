using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KidsOrganizationApp.Domain
{
    public class PersonInfo
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }
        public Contact? Contact { get; set; }

        public Guid PassportId { get; set; }
        public Document? Passport { get; set; }

        public Guid SNILSId { get; set; }
        public Document? SNILS { get; set; }

        public Guid DiagnosisFileId { get; set; }
        public Document? DiagnosisFile { get; set; } 

        public string MembershipStatus { get; set; } = string.Empty;
    }
}
