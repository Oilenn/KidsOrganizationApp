using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IEventService
    {
        Task<EventDTO> AddAsync(EventDTO dto, CancellationToken cancellationToken = default);
        Task<EventDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<EventDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(EventDTO dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
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

        public async Task<EventDTO> AddAsync(EventDTO dto, CancellationToken cancellationToken = default)
        {
            var ev = _mapper.ToNewDomain(dto);

            await _eventRepository.AddAsync(ev, cancellationToken);
            return _mapper.ToDTO(ev);
        }

        public async Task<List<EventDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);
            return events
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public async Task<EventDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var ev = await _eventRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.ToDTO(ev!);
        }

        public async Task UpdateAsync(EventDTO dto, CancellationToken cancellationToken = default)
        {
            var ev = await _eventRepository.GetByIdAsync(dto.Id, cancellationToken);
            _mapper.UpdateDomain(ev!, dto);

            await _eventRepository.UpdateAsync(ev!, cancellationToken);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _eventRepository.RemoveAsync(id, cancellationToken);
        }
    }
}
