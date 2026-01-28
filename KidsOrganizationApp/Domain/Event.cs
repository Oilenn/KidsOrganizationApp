using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Event
    {
        public Guid Id { get; set; }    
        public string Name { get; set; } = string.Empty;
        public List<Document> Document { get; set; } = new List<Document>();
    }
}
