using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }
        public string DocumentType { get; set; }
        public string Path { get; set; }
    }

}
