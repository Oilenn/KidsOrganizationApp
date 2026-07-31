using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service;

public interface IChildService
{
    ChildDTO AddChild(ChildDTO dto);
    ChildDTO GetChildById(Guid id);
    void DeleteChild(Guid id);
    void UpdateChild(ChildDTO dto);
    List<ChildDTO> GetAllChildren();
    List<ChildDTO> GetChildrenByName(string name);
    List<ChildDTO> GetChildrenBySurname(string surname);
    List<ChildDTO> GetChildrenByPatronymic(string patronymic);
    List<ParentDTO> GetParents(ChildDTO dto);
    void AddParent(ParentDTO parent, ChildDTO dto);
}

public class ChildService : IChildService
{
    private readonly IChildRepository _children;
    private readonly IParentRepository _parents;
    private readonly IParentService _parentService;
    private readonly IMapper<ChildDTO, Child> _mapper;

    public ChildService(IChildRepository children, IParentRepository parents, IParentService parentService, ChildMapper mapper)
    {
        _children = children; _parents = parents; _parentService = parentService; _mapper = mapper;
    }

    public ChildDTO AddChild(ChildDTO dto)
    {
        var child = _mapper.ToNewDomain(dto);
        foreach (var parentId in dto.ParentIds) child.AddParent(_parents.GetById(parentId));
        _children.Add(child);
        return _mapper.ToDTO(child);
    }

    public ChildDTO GetChildById(Guid id) => _mapper.ToDTO(_children.GetById(id));
    public List<ChildDTO> GetAllChildren() => _mapper.ToDTO(_children.GetAll());
    public List<ChildDTO> GetChildrenByName(string name) => _mapper.ToDTO(_children.GetByName(name));
    public List<ChildDTO> GetChildrenBySurname(string surname) => _mapper.ToDTO(_children.GetBySurname(surname));
    public List<ChildDTO> GetChildrenByPatronymic(string patronymic) => _mapper.ToDTO(_children.GetByPatronymic(patronymic));
    public void DeleteChild(Guid id) => _children.Remove(id);
    public void UpdateChild(ChildDTO dto) { var child = _children.GetById(dto.Id); _mapper.UpdateDomain(child, dto); _children.Update(child); }

    public void AddParent(ParentDTO parent, ChildDTO dto)
    {
        var child = _children.GetById(dto.Id);
        var savedParent = _parentService.Add(parent);
        child.AddParent(_parents.GetById(savedParent.Id));
        _children.Update(child);
    }

    public List<ParentDTO> GetParents(ChildDTO dto) => dto.ParentIds.Select(_parentService.GetParentById).ToList();
}
