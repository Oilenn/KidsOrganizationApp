using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service;

public enum DocumentOwnerType { Child, Parent, Event }

public interface IDocumentService
{
    DocumentDTO Add(DocumentDTO dto);
    DocumentDTO AddToOwner(DocumentDTO dto, DocumentOwnerType ownerType, Guid ownerId);
    List<DocumentDTO> GetAll();
    List<DocumentDTO> GetByIds(IEnumerable<Guid> ids);
    DocumentDTO GetById(Guid id);
    void Update(DocumentDTO dto);
    void Delete(Guid id);
}

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documents;
    private readonly IChildRepository _children;
    private readonly IParentRepository _parents;
    private readonly IEventRepository _events;
    private readonly IMapper<DocumentDTO, Document> _mapper;

    public DocumentService(IDocumentRepository documents, IChildRepository children, IParentRepository parents, IEventRepository events, DocumentMapper mapper)
    {
        _documents = documents; _children = children; _parents = parents; _events = events; _mapper = mapper;
    }

    public DocumentDTO Add(DocumentDTO dto)
    {
        var document = _mapper.ToNewDomain(dto);
        _documents.Add(document);
        return _mapper.ToDTO(document);
    }

    public DocumentDTO AddToOwner(DocumentDTO dto, DocumentOwnerType ownerType, Guid ownerId)
    {
        var document = _mapper.ToNewDomain(dto);
        _documents.Add(document);
        switch (ownerType)
        {
            case DocumentOwnerType.Child:
                var child = _children.GetById(ownerId) ?? throw new KeyNotFoundException("Ребёнок не найден.");
                child.Documents.Add(document);
                _children.Update(child);
                break;
            case DocumentOwnerType.Parent:
                var parent = _parents.GetById(ownerId) ?? throw new KeyNotFoundException("Родитель не найден.");
                parent.AddDocument(document);
                _parents.Update(parent);
                break;
            case DocumentOwnerType.Event:
                var eventEntity = _events.GetById(ownerId) ?? throw new KeyNotFoundException("Мероприятие не найдено.");
                eventEntity.AddDocument([document]);
                _events.Update(eventEntity);
                break;
        }
        return _mapper.ToDTO(document);
    }

    public List<DocumentDTO> GetAll() => _mapper.ToDTO(_documents.GetAll());
    public List<DocumentDTO> GetByIds(IEnumerable<Guid> ids) => ids.Distinct().Select(GetById).Where(document => document is not null).ToList();
    public DocumentDTO GetById(Guid id) => _mapper.ToDTO(_documents.GetById(id));
    public void Update(DocumentDTO dto) { var document = _documents.GetById(dto.Id); _mapper.UpdateDomain(document, dto); _documents.Update(document); }
    public void Delete(Guid id) => _documents.Remove(id);
}
