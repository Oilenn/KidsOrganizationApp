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
        public DateTime Date { get; private set; } = DateTime.MinValue;
        public List<Document> Document { get; set; } = new List<Document>();
    }
}
