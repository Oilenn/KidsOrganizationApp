using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    public class EventDTO : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<Guid> Documents { get; set; } = new List<Guid>();

        public EventDTO(Guid id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }
    }
}