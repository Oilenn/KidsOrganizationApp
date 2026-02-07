using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Parent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Surname { get; private set; } = string.Empty;
        public string Patronymic { get; private set; } = string.Empty;
        public DateTime DateBirth { get; private set; } = DateTime.MinValue;
        public List<Child> Children { get; private set; } = [];

        private const int MinAge = 14;

        protected Parent() { }

        public Parent(string name,
              string surname,
              string patronomic,
              DateTime dateBirth) 
            : this(name, surname, patronomic, dateBirth, []) { }

        public Parent(string name,
              string surname,
              string patronomic,
              DateTime dateBirth,
              Child child)
            : this(name, surname, patronomic, dateBirth, [child]) { }

        public Parent(string name,
              string surname,
              string patronomic,
              DateTime dateBirth,
              List<Child> children)
        {
            ChangeName(name, surname, patronomic);
            ChangeDateBirth(dateBirth);
            AddChildren(children);

            Id = Guid.NewGuid();
        }

        //todo: сделать инвариант для изменения каждого ФИО(для ребенка также)
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

        public void AddChildren(List<Child> childen)
        {
            foreach (var child in childen)
            {
                if (child.Parents.Contains(this)) throw new ArgumentException("Родитель уже является родителем!");

                Children.Add(child);
                child.AddParents([this]);
            }
        }

        public void ChangeDateBirth(DateTime dateBirth)
        {
            if (MinAge < DateTime.Now.Year - dateBirth.Year)
                throw new ArgumentException("Дата рождения не может превышать 14 лет!");

            DateBirth = dateBirth;
        }
    }
}
