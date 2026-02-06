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
        public string DocumentType { get; private set; } = string.Empty;
        public string Path { get; private set; } = string.Empty;

        protected Document() { }
    }
}
