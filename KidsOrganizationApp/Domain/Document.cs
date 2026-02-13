using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Document
    {
        public Guid Id { get; private set; }
        public DocumentType Type { get; private set; } = DocumentType.Unknown;
        public string Path { get; private set; } = string.Empty;

        protected Document() { }

        public Document(DocumentType documentType, string path)
        {
            Id = Guid.NewGuid();

            ChangeType(documentType);
            ChangePath(path);
        }

        public void ChangePath(string path)
        {
            Path = path.ToLower();//todo: доделать инвариант пути
        }

        public void ChangeType(DocumentType type)
        {
            Type = type;
        }

        public enum DocumentType
        {
            Unknown = 0,
            Passport = 1,
            SNILS = 2,
            Diagnosis = 3,
            
            Letter = 4,
            Order = 5
        }
    }
}
