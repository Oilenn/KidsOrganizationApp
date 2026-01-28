using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Document
    {
        public Guid Id { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
