using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Parent : IDomain
    {
        public Guid Id { get; private set; }

        public FullName FullName { get; set; }
        public Contact Contact { get; set; }

        public DateTime DateBirth { get; private set; } = DateTime.MinValue;
        public MembershipStatus MembershipStatus { get; set; } = MembershipStatus.Active;

        public List<Document> Documents = new List<Document>();

        public const int MinAge = 16;

        protected Parent() { }

        public Parent(FullName fullName,
                      Contact contact,
                      DateTime dateBirth,
                      List<Document> documents)
        {
            FullName = fullName;
            Contact = contact;

            Id = Guid.NewGuid();
        }

        public void ChangeDateBirth(DateTime birth)
        {
            if (birth.Year < MinAge)
                throw new ArgumentException($"Родитель не может быть младше {MinAge} лет!");

            DateBirth = birth;
        }

        public void AddDocument(Document document)
        {
            Documents.Add(document);
        }

        public void AddDocuments(List<Document> documents)
        {
            foreach(Document document in documents)
            {
                AddDocument(document);
            }
        }
    }
}
