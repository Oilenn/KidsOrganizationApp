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
        Task AddParentAndChildAsync(ChildDTO child, ParentDTO parent, CancellationToken cancellationToken = default);
        Task DeleteChildWithParentsAsync(Guid child, CancellationToken cancellationToken = default);
    }

    public class FamilyService : IFamilyService
    {
        private readonly IChildService _childService;
        private readonly IParentService _parentService;

        public FamilyService(
            IChildService childService,
            IParentService parentService)
        {
            _childService = childService;
            _parentService = parentService;
        }

        public async Task AddParentAndChildAsync(
            ChildDTO child,
            ParentDTO parent,
            CancellationToken cancellationToken = default)
        {
            await _childService.AddChildAsync(child, cancellationToken);
            await _childService.AddParentAsync(parent, child, cancellationToken);
        }

        public async Task DeleteChildWithParentsAsync(Guid childId, CancellationToken cancellationToken = default)
        {
            var child = await _childService.GetChildByIdAsync(childId, cancellationToken);
            List<ParentDTO> parents = await _childService.GetParentsAsync(child, cancellationToken);

            foreach (ParentDTO parent in parents)
            {
                await _parentService.DeleteAsync(parent.Id, cancellationToken);
            }

            await _childService.DeleteChildAsync(childId, cancellationToken);
        }
    }
}
