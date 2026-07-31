using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service
{
    public interface IFamilyService
    {
        void AddParentAndChild(ChildDTO child, ParentDTO parent);
        void DeleteChildWithParents(Guid child);
    }

    public class FamilyService : IFamilyService
    {
        private readonly IChildService _childService;
        private readonly IParentService _parentService;

        public FamilyService(IChildService childService,
                            IParentService parentService) 
        {
            _childService = childService;
            _parentService = parentService;
        }

        public void AddParentAndChild(ChildDTO child, ParentDTO parent)
        {
            _childService.AddChild(child);
            _childService.AddParent(parent, child);
        }

        public void DeleteChildWithParents(Guid childId)
        {
            List<ParentDTO> parents = _childService.GetParents(_childService.GetChildById(childId));

            foreach (ParentDTO parent in parents)
            {
                _parentService.Delete(parent.Id);
            }

            _childService.DeleteChild(childId);
        }
    }
}
