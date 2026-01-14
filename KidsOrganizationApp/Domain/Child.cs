using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Child
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Surname { get; private set; } = string.Empty;
        public string Patronomic { get; private set; } = string.Empty;
        public DateTime DateBirth { get; private set; } = DateTime.MinValue;

        public List<Parent> Parents { get; set; } = [];

        public Child(string name,
                      string surname,
                      string patronomic,
                      DateTime dateBirth)
        {
            ChangeName(name, surname, patronomic);
            DateBirth = dateBirth;
            Id = Guid.NewGuid();
        }

        public void ChangeName(string name, string surname, string patronomic)
        {
            if (string.IsNullOrWhiteSpace(name) || 
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(patronomic))
                throw new ArgumentException("Имя не может быть пустым");

            Name = name;
            Surname = surname;
            Patronomic = patronomic;
        }
    }

}
