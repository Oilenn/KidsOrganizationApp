using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

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
        private readonly IMapper<EventDTO, Event> _mapper;

        public EventService(IEventRepository eventRepository, EventMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public EventDTO Add(EventDTO dto)
        {
            var ev = _mapper.ToNewDomain(dto);

            _eventRepository.Add(ev);
            return _mapper.ToDTO(ev);
        }

        public List<EventDTO> GetAll()
        {
            return _eventRepository.GetAll()
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public EventDTO GetById(Guid id)
        {
            return _mapper.ToDTO(_eventRepository.GetById(id));
        }

        public void Update(EventDTO dto)
        {
            _eventRepository.Update(_eventRepository.GetById(dto.Id));
        }

        public void Delete(EventDTO dto)
        {
            _eventRepository.Remove(_eventRepository.GetById(dto.Id));
        }
    }
}
