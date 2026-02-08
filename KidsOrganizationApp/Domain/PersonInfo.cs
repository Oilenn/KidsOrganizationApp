using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KidsOrganizationApp.Domain
{
    public class PersonInfo : IDomain
    {
        public Guid Id { get; private set; }

        public Guid ContactId { get; private set; }
        public Contact? Contact { get; set; }

        public Guid PassportId { get; private set; }
        public Document? Passport { get; set; }

        public Guid SNILSId { get; private set; }
        public Document? SNILS { get; set; }

        public Guid DiagnosisFileId { get; private set; }
        public Document? DiagnosisFile { get; set; } 

        public MembershipStatus MembershipStatus { get; private set; } = MembershipStatus.Active;

        protected PersonInfo() { }

        public PersonInfo(Contact contact, Document passport, Document SNILs, Document diagnosis, MembershipStatus status)
        {
            Contact = contact;
            Passport = passport;
            SNILS = SNILs;
            DiagnosisFile = diagnosis;
            MembershipStatus = status;
        }

        public void AddContact(Contact contact)
        {
            Contact = contact;
        }
    }

    public enum MembershipStatus
    {
        Active = 0,
        Left = 1
    }
}
