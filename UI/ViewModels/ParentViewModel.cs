using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.UI.ViewModels
{
    public class ParentViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronomic { get; }
        public string DateOfBirth { get; }

        public ParentViewModel(Domain.Parent parent)
        {
            Id = parent.Id;
            Name = parent.Name;
            Surname = parent.Surname;
            Patronomic = parent.Patronymic;
            DateOfBirth = parent.DateBirth.ToString("dd.MM.yyyy");
        }
    }
}
