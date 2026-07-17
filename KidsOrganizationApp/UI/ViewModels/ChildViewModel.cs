using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.UI.ViewModels
{
    public class ChildViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronomic { get; }
        public string DateOfBirth { get; }

        public ChildViewModel(Domain.Child child)
        {
            Id = child.Id;
            Name = child.Name;
            Surname = child.Surname;
            Patronomic = child.Patronymic;
            DateOfBirth = child.DateBirth.ToString("dd.MM.yyyy");
        }
    }
}

