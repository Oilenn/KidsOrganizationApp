using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Event
    {
        public Guid Id { get; private set; }    
        public string Name { get; private set; } = string.Empty;
        public DateTime Date { get; private set; } = DateTime.MinValue;
        public List<Document> Document { get; private set; } = new List<Document>();

        protected Event() { }
    }
}
