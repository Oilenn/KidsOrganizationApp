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
    }
}