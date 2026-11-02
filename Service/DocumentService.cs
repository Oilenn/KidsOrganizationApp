using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service;

public enum DocumentOwnerType
{
    Child,
    Parent,
    Event
}

public interface IDocumentService
{
    Task<DocumentDTO> AddAsync(DocumentDTO dto, CancellationToken cancellationToken = default);
    Task<DocumentDTO> AddToOwnerAsync(
        DocumentDTO dto,
        DocumentOwnerType ownerType,
        Guid ownerId,
        CancellationToken cancellationToken = default);
    Task<List<DocumentDTO>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<DocumentDTO>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<DocumentDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(DocumentDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documents;
    private readonly IChildRepository _children;
    private readonly IParentRepository _parents;
    private readonly IEventRepository _events;
    private readonly IMapper<DocumentDTO, Document> _mapper;

    public DocumentService(
        IDocumentRepository documents,
        IChildRepository children,
        IParentRepository parents,
        IEventRepository events,
        DocumentMapper mapper)
    {
        _documents = documents;
        _children = children;
        _parents = parents;
        _events = events;
        _mapper = mapper;
    }

    public async Task<DocumentDTO> AddAsync(DocumentDTO dto, CancellationToken cancellationToken = default)
    {
        var document = _mapper.ToNewDomain(dto);
        await _documents.AddAsync(document, cancellationToken);
        return _mapper.ToDTO(document);
    }

    public async Task<DocumentDTO> AddToOwnerAsync(
        DocumentDTO dto,
        DocumentOwnerType ownerType,
        Guid ownerId,
        CancellationToken cancellationToken = default)
    {
        var document = _mapper.ToNewDomain(dto);
        await _documents.AddAsync(document, cancellationToken);
        switch (ownerType)
        {
            case DocumentOwnerType.Child:
                var child = await _children.GetByIdAsync(ownerId, cancellationToken)
                    ?? throw new KeyNotFoundException("Ребёнок не найден.");
                child.Documents.Add(document);
                await _children.UpdateAsync(child, cancellationToken);
                break;
            case DocumentOwnerType.Parent:
                var parent = await _parents.GetByIdAsync(ownerId, cancellationToken)
                    ?? throw new KeyNotFoundException("Родитель не найден.");
                parent.AddDocument(document);
                await _parents.UpdateAsync(parent, cancellationToken);
                break;
            case DocumentOwnerType.Event:
                var eventEntity = await _events.GetByIdAsync(ownerId, cancellationToken)
                    ?? throw new KeyNotFoundException("Мероприятие не найдено.");
                eventEntity.AddDocument([document]);
                await _events.UpdateAsync(eventEntity, cancellationToken);
                break;
        }
        return _mapper.ToDTO(document);
    }

    public async Task<List<DocumentDTO>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _documents.GetAllAsync(cancellationToken);
        return _mapper.ToDTO(documents);
    }

    public async Task<List<DocumentDTO>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var documents = new List<DocumentDTO>();
        foreach (var id in ids.Distinct())
        {
            var document = await GetByIdAsync(id, cancellationToken);
            if (document is not null)
                documents.Add(document);
        }
        return documents;
    }

    public async Task<DocumentDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetByIdAsync(id, cancellationToken);
        return _mapper.ToDTO(document!);
    }

    public async Task UpdateAsync(DocumentDTO dto, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetByIdAsync(dto.Id, cancellationToken);
        _mapper.UpdateDomain(document!, dto);
        await _documents.UpdateAsync(document!, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        _documents.RemoveAsync(id, cancellationToken);
}
