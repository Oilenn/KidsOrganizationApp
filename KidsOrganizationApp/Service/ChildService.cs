using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Repository;
using KidsOrganizationApp.Repository.Interface;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.Service.Mapper;

namespace KidsOrganizationApp.Service
{
    public interface IChildService
    {
        ChildDTO AddChild(ChildDTO dto);
        ChildDTO GetChildById(Guid id);

        void DeleteChild(ChildDTO dto);
        void UpdateChild(ChildDTO dto);

        List<ChildDTO> GetAllChildren();
        List<ChildDTO> GetChildrenByName(string name);
        List<ChildDTO> GetChildrenBySurname(string surname);
        List<ChildDTO> GetChildrenByPatronymic(string patronomyc);

        List<ParentDTO> GetParents(ChildDTO dto);
        void AddParent(ParentDTO parent, ChildDTO dto);
    }

    public class ChildService : IChildService
    {
        private IChildRepository _childRepository;
        private IParentService _parentSerivce;
        private IMapper<ChildDTO, Child> _mapper;

        public ChildService(IChildRepository childRepository, IParentService parentService, ChildMapper mapper) 
        {
            _childRepository = childRepository;
            _parentSerivce = parentService;
            _mapper = mapper;
        }

        public ChildDTO AddChild(ChildDTO dto)
        {
            Child child = _mapper.ToNewDomain(dto);

            _childRepository.Add(child);

            return _mapper.ToDTO(child);
        }

        public List<ChildDTO> GetAllChildren()
        {
            List<ChildDTO> children = _mapper.ToDTO(_childRepository.GetAll());

            return children;
        }

        public ChildDTO GetChildById(Guid id)
        {
            return _mapper.ToDTO(_childRepository.GetById(id));
        }

        public List<ChildDTO> GetChildrenByName(string name)
        {
            return _mapper.ToDTO(_childRepository.GetByName(name));
        }

        public List<ChildDTO> GetChildrenBySurname(string surname)
        {
            return _mapper.ToDTO(_childRepository.GetBySurname(surname));
        }

        public List<ChildDTO> GetChildrenByPatronymic(string patronomyc)
        {
            return _mapper.ToDTO(_childRepository.GetByPatronymic(patronomyc));
        }

        public List<ParentDTO> GetParents(ChildDTO dto)
        {
            return new List<ParentDTO>() {
                _parentSerivce.GetParentById(dto.ParentIds[0])
            };
        }

        public void DeleteChild(ChildDTO dto)
        {
            _childRepository.Remove(dto.Id);
        }

        public void UpdateChild(ChildDTO dto)
        {
            var child = _childRepository.GetById(dto.Id);
            _mapper.UpdateDomain(child, dto);

            _childRepository.Update(child);
        }

        public void AddParent(ParentDTO parent, ChildDTO dto)
        {
            var par = _parentSerivce.Add(parent);
            dto.ParentIds.Add(par.Id);

            UpdateChild(dto);
        }
    }
}
