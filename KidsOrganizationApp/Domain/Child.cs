using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Child : IDomain
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Surname { get; private set; } = string.Empty;
        public string Patronymic { get; private set; } = string.Empty;
        public DateTime DateBirth { get; private set; } = DateTime.MinValue;

        public List<Parent> Parents { get; set; } = [];

        private const int MaxParents = 2;

        protected Child() { }

        public Child(string name,
                      string surname,
                      string patronymic,
                      DateTime dateBirth) : this(name, surname, patronymic, dateBirth, []) { }

        public Child(string name,
              string surname,
              string patronymic,
              DateTime dateBirth,
              Parent parent) : this(name, surname, patronymic, dateBirth, [parent]) { }

        public Child(string name,
              string surname,
              string patronymic,
              DateTime dateBirth,
              List<Parent> parents)
        {
            ChangeName(name, surname, patronymic);
            ChangeDateBirth(dateBirth);
            AddParents(parents);

            Id = Guid.NewGuid();
        }

        public void ChangeName(string name, string surname, string patronymic)
        {
            if (string.IsNullOrWhiteSpace(name) || 
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(patronymic))
                throw new ArgumentException("Имя не может быть пустым");

            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }

        public void ChangeDateBirth(DateTime dateBirth)
        {
            if (dateBirth > DateTime.Now) 
                throw new ArgumentException("Дата рождения не может превышать сегодняшний день!");

            DateBirth = dateBirth;
        }

        public void AddParents(List<Parent> parents)
        {
            if (Parents.Count > MaxParents) throw new ArgumentException("Родителей не может быть больше 2!");
            foreach (Parent parent in parents)
            {
                if (parent.Children.Contains(this)) throw new ArgumentException("Ребенок уже является ребенком!");

                Parents.Add(parent);
                parent.AddChildren([this]);
            }
        }
    }
}
