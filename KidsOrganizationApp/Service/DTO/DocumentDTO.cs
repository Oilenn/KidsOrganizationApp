using KidsOrganizationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    public class DocumentDTO : IDTO
    {
        public Guid Id { get; set; }
        public Document.DocumentType Type { get; set; }
        public string Path { get; set; }
        public string TypeName => Type switch
        {
            Document.DocumentType.Passport => "Паспорт",
            Document.DocumentType.SNILS => "СНИЛС",
            Document.DocumentType.Diagnosis => "Диагноз",
            Document.DocumentType.Letter => "Письмо",
            Document.DocumentType.Order => "Приказ",
            _ => "Не указан"
        };

        public DocumentDTO(Guid id, Document.DocumentType type, string path)
        {
            Id = id;
            Type = type;
            Path = path;
        }
    }
}
