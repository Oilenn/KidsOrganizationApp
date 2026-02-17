using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;

namespace KidsOrganizationApp.Service.Mapper
{
    public class EventMapper : IMapper<EventDTO, Event>
    {
        private readonly DocumentMapper _documentMapper;

        public EventMapper(DocumentMapper documentMapper)
        {
            _documentMapper = documentMapper;
        }

        public EventDTO ToDTO(Event eventD)
        {
            return new EventDTO(eventD.Id, eventD.Name, eventD.Date);
        }

        public List<EventDTO> ToDTO(List<Event> events)
        {
            var dto = new List<EventDTO>();

            foreach (var @event in events)
                dto.Add(ToDTO(@event));

            return dto;
        }

        public Event ToNewDomain(EventDTO dto)
        {
            var newEvent = new Event(
                dto.Name,
                dto.Date,
                new List<Document>());

            return newEvent;
        }

        public void UpdateDomain(Event domain, EventDTO dto)
        {
            domain.ChangeDate(dto.Date);
            domain.ChangeName(dto.Name);
        }
    }
}
