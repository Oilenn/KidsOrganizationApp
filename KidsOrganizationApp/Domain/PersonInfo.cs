using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KidsOrganizationApp.Domain
{
    public class PersonInfo
    {
        public Guid Id { get; private set; }

        public Guid ContactId { get; private set; }
        public Contact? Contact { get; private set; }

        public Guid PassportId { get; private set; }
        public Document? Passport { get; private set; }

        public Guid SNILSId { get; private set; }
        public Document? SNILS { get; private set; }

        public Guid DiagnosisFileId { get; private set; }
        public Document? DiagnosisFile { get; private set; } 

        public string MembershipStatus { get; private set; } = string.Empty;

        protected PersonInfo() { }

        public PersonInfo(Contact contact, Document passport, Document SNILs, Document diagnosis, string membershipStatus)
        {
            Contact = contact;
            Passport = passport;
            SNILS = SNILs;
            DiagnosisFile = diagnosis;
            MembershipStatus = membershipStatus;
        }
    }
}
