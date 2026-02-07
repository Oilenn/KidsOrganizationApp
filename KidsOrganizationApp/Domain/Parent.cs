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
        public ICollection<Child> Children { get; private set; } = [];  

        protected Parent() { }

        public Parent(string name,
                      string surname,
                      string patronomic,
                      DateTime dateBirth)
        {
            ChangeName(name, surname, patronomic);
            DateBirth = dateBirth;
            Id = Guid.NewGuid();
        }

        public Parent(string name,
              string surname,
              string patronomic,
              DateTime dateBirth,
              List<Child> children)
        {
            ChangeName(name, surname, patronomic);
            DateBirth = dateBirth;
            Id = Guid.NewGuid();

            Children = children;
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

        public void AddChild(Child child)
        {
            if(child.Parents.Contains(this)) throw new ArgumentException("Родитель уже является родителем!");
            
            Children.Add(child);
            child.AddParent(this);
        }

        public void AddDateBirth(DateTime dateBirth)
        {
            if(dateBirth > DateTime.Now) throw new ArgumentException("Дата рождения не может превышать сегодняшний день!");

            DateBirth = dateBirth;
        }
    }
}
