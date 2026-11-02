using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service;

public interface IChildService
{
    Task<ChildDTO> AddChildAsync(ChildDTO dto, CancellationToken cancellationToken = default);
    Task<ChildDTO> GetChildByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteChildAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateChildAsync(ChildDTO dto, CancellationToken cancellationToken = default);
    Task<List<ChildDTO>> GetAllChildrenAsync(CancellationToken cancellationToken = default);
    Task<List<ChildDTO>> GetChildrenByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<ChildDTO>> GetChildrenBySurnameAsync(string surname, CancellationToken cancellationToken = default);
    Task<List<ChildDTO>> GetChildrenByPatronymicAsync(string patronymic, CancellationToken cancellationToken = default);
    Task<List<ParentDTO>> GetParentsAsync(ChildDTO dto, CancellationToken cancellationToken = default);
    Task AddParentAsync(ParentDTO parent, ChildDTO dto, CancellationToken cancellationToken = default);
}

public class ChildService : IChildService
{
    private readonly IChildRepository _children;
    private readonly IParentRepository _parents;
    private readonly IParentService _parentService;
    private readonly IMapper<ChildDTO, Child> _mapper;

    public ChildService(
        IChildRepository children,
        IParentRepository parents,
        IParentService parentService,
        ChildMapper mapper)
    {
        _children = children;
        _parents = parents;
        _parentService = parentService;
        _mapper = mapper;
    }

    public async Task<ChildDTO> AddChildAsync(ChildDTO dto, CancellationToken cancellationToken = default)
    {
        var child = _mapper.ToNewDomain(dto);
        foreach (var parentId in dto.ParentIds)
            child.AddParent((await _parents.GetByIdAsync(parentId, cancellationToken))!);
        await _children.AddAsync(child, cancellationToken);
        return _mapper.ToDTO(child);
    }

    public async Task<ChildDTO> GetChildByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var child = await _children.GetByIdAsync(id, cancellationToken);
        return _mapper.ToDTO(child!);
    }

    public async Task<List<ChildDTO>> GetAllChildrenAsync(CancellationToken cancellationToken = default)
    {
        var children = await _children.GetAllAsync(cancellationToken);
        return _mapper.ToDTO(children);
    }

    public async Task<List<ChildDTO>> GetChildrenByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var children = await _children.GetByNameAsync(name, cancellationToken);
        return _mapper.ToDTO(children);
    }

    public async Task<List<ChildDTO>> GetChildrenBySurnameAsync(string surname, CancellationToken cancellationToken = default)
    {
        var children = await _children.GetBySurnameAsync(surname, cancellationToken);
        return _mapper.ToDTO(children);
    }

    public async Task<List<ChildDTO>> GetChildrenByPatronymicAsync(
        string patronymic,
        CancellationToken cancellationToken = default)
    {
        var children = await _children.GetByPatronymicAsync(patronymic, cancellationToken);
        return _mapper.ToDTO(children);
    }

    public Task DeleteChildAsync(Guid id, CancellationToken cancellationToken = default) =>
        _children.RemoveAsync(id, cancellationToken);

    public async Task UpdateChildAsync(ChildDTO dto, CancellationToken cancellationToken = default)
    {
        var child = await _children.GetByIdAsync(dto.Id, cancellationToken);
        _mapper.UpdateDomain(child!, dto);
        await _children.UpdateAsync(child!, cancellationToken);
    }

    public async Task AddParentAsync(ParentDTO parent, ChildDTO dto, CancellationToken cancellationToken = default)
    {
        var child = await _children.GetByIdAsync(dto.Id, cancellationToken);
        var savedParent = await _parentService.AddAsync(parent, cancellationToken);
        child!.AddParent((await _parents.GetByIdAsync(savedParent.Id, cancellationToken))!);
        await _children.UpdateAsync(child, cancellationToken);
    }

    public async Task<List<ParentDTO>> GetParentsAsync(ChildDTO dto, CancellationToken cancellationToken = default)
    {
        var parents = new List<ParentDTO>();
        foreach (var parentId in dto.ParentIds)
            parents.Add(await _parentService.GetParentByIdAsync(parentId, cancellationToken));
        return parents;
    }
}
