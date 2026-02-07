using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;

namespace KidsOrganizationApp.Service
{
    public interface IEventService
    {
        EventDTO Add(EventDTO dto);
        EventDTO GetById(Guid id);
        List<EventDTO> GetAll();
        void Update(EventDTO dto);
        void Delete(EventDTO dto);
    }

    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public EventDTO Add(EventDTO dto)
        {
            var ev = new Event(
                dto.Name,
                dto.Date,
                dto.Documents.Select(
                    d => new Document(d.DocumentType, d.Path)).ToList()
            );

            _eventRepository.Add(ev);
            return ConvertToDTO(ev);
        }

        public List<EventDTO> GetAll()
        {
            return _eventRepository.GetAll()
                .Select(ConvertToDTO)
                .ToList();
        }

        public EventDTO GetById(Guid id)
        {
            return ConvertToDTO(_eventRepository.GetById(id));
        }

        public void Update(EventDTO dto)
        {
            _eventRepository.Update(_eventRepository.GetById(dto.Id));
        }

        public void Delete(EventDTO dto)
        {
            _eventRepository.Remove(_eventRepository.GetById(dto.Id));
        }

        private EventDTO ConvertToDTO(Event ev)
        {
            return new EventDTO
            {
                Id = ev.Id,
                Name = ev.Name,
                Date = ev.Date,
                Documents = ev.Documents.Select(d => new DocumentDTO
                {
                    Id = d.Id,
                    DocumentType = d.Type,
                    Path = d.Path
                }).ToList()
            };
        }
    }
}
