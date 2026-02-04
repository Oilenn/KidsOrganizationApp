using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    public class ParentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; } = DateTime.MinValue;
        public List<ChildDTO> Children { get; set; } = [];

        public ParentDTO() { }

        public ParentDTO(Guid id, string name, string surname, string patronymic, DateTime dateBirth, List<ChildDTO> children)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            DateBirth = dateBirth;
            Children = children;
        }
    }
}
