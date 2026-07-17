using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IDocumentService
    {
        DocumentDTO Add(DocumentDTO dto);
        DocumentDTO AddToOwner(DocumentDTO dto, DocumentOwnerType ownerType, Guid ownerId);
        List<DocumentDTO> GetAll();
        DocumentDTO GetById(Guid id);
        List<DocumentDTO> GetByIds(IEnumerable<Guid> ids);
        void Update(DocumentDTO dto);
        void Delete(Guid id);
    }

    public enum DocumentOwnerType { Child, Parent, Event }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper<DocumentDTO, Document> _mapper;
        private readonly IChildRepository _childRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IEventRepository _eventRepository;

        public DocumentService(IDocumentRepository documentRepository, DocumentMapper mapper,
            IChildRepository childRepository, IParentRepository parentRepository, IEventRepository eventRepository)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _childRepository = childRepository;
            _parentRepository = parentRepository;
            _eventRepository = eventRepository;
        }

        public DocumentDTO Add(DocumentDTO dto)
        {
            var document = _mapper.ToNewDomain(dto);
            _documentRepository.Add(document);
            return _mapper.ToDTO(document);
        }

        public DocumentDTO AddToOwner(DocumentDTO dto, DocumentOwnerType ownerType, Guid ownerId)
        {
            var document = _mapper.ToNewDomain(dto);
            switch (ownerType)
            {
                case DocumentOwnerType.Child:
                    var child = _childRepository.GetById(ownerId) ?? throw new ArgumentException("Ребенок не найден.");
                    child.Documents.Add(document);
                    _childRepository.Update(child);
                    break;
                case DocumentOwnerType.Parent:
                    var parent = _parentRepository.GetById(ownerId) ?? throw new ArgumentException("Родитель не найден.");
                    parent.Documents.Add(document);
                    _parentRepository.Update(parent);
                    break;
                case DocumentOwnerType.Event:
                    var eventEntity = _eventRepository.GetById(ownerId) ?? throw new ArgumentException("Мероприятие не найдено.");
                    eventEntity.Documents.Add(document);
                    _eventRepository.Update(eventEntity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ownerType));
            }
            return _mapper.ToDTO(document);
        }

        public List<DocumentDTO> GetAll()
        {
            return _documentRepository.GetAll()
                .Select(_mapper.ToDTO)
                .ToList();
        }

        public DocumentDTO GetById(Guid id)
        {
            return _mapper.ToDTO(_documentRepository.GetById(id));
        }

        public List<DocumentDTO> GetByIds(IEnumerable<Guid> ids)
        {
            var idSet = ids.ToHashSet();
            return GetAll().Where(d => idSet.Contains(d.Id)).ToList();
        }

        public void Update(DocumentDTO dto)
        {
            var document = _documentRepository.GetById(dto.Id);
            _mapper.UpdateDomain(document, dto);

            _documentRepository.Update(document);
        }

        public void Delete(Guid id)
        {
            _documentRepository.Remove(id);
        }
    }
}

