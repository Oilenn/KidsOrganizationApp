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

        public EventDTO ToDTO(Event @event)
        {
            return new EventDTO
            {
                Id = @event.Id,
                Name = @event.Name,
                Date = @event.Date,
                Documents = _documentMapper.ToDTO(@event.Documents)
            };
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
            var documents = new List<Document>();

            foreach (var documentDto in dto.Documents)
                documents.Add(_documentMapper.ToNewDomain(documentDto));

            return new Event(
                dto.Name,
                dto.Date,
                documents
            );
        }
    }
}
