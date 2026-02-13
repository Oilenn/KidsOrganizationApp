using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service.DTO;
using System;
using System.Collections.Generic;

namespace KidsOrganizationApp.Service.Mapper
{
    public class ParentMapper : IMapper<ParentDTO, Parent>
    {
        private readonly ChildMapper _childMapper;

        public ParentMapper(ChildMapper childMapper)
        {
            _childMapper = childMapper;
        }

        public ParentDTO ToDTO(Parent parent)
        {
            return new ParentDTO
            {
                Id = parent.Id,
                Name = parent.Name,
                Surname = parent.Surname,
                Patronymic = parent.Patronymic,
                DateBirth = parent.DateBirth,
                Children = _childMapper.ToDTO(parent.Children)
            };
        }

        public List<ParentDTO> ToDTO(List<Parent> parents)
        {
            var dto = new List<ParentDTO>();

            foreach (var parent in parents)
                dto.Add(ToDTO(parent));

            return dto;
        }

        public Parent ToNewDomain(ParentDTO dto)
        {
            var children = new List<Child>();

            foreach (var childDto in dto.Children)
                children.Add(_childMapper.ToNewDomain(childDto));

            return new Parent(
                dto.Name,
                dto.Surname,
                dto.Patronymic,
                dto.DateBirth,
                children
            );
        }
    }
}
