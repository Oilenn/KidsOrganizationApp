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
        public string Patronymic { get; private set; } = string.Empty;
        public DateTime DateBirth { get; private set; } = DateTime.MinValue;

        public List<Parent> Parents { get; set; } = [];

        protected Child() { }


        public Child(string name,
                      string surname,
                      string patronymic,
                      DateTime dateBirth)
        {
            ChangeName(name, surname, patronymic);
            DateBirth = dateBirth;
            Id = Guid.NewGuid();
        }

        public Child(string name,
              string surname,
              string patronymic,
              DateTime dateBirth,
              Parent parent)
        {
            ChangeName(name, surname, patronymic);
            DateBirth = dateBirth;
            Id = Guid.NewGuid();
        }

        public Child(string name,
              string surname,
              string patronymic,
              DateTime dateBirth,
              List<Parent> parents)
        {
            ChangeName(name, surname, patronymic);
            DateBirth = dateBirth;
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

        public void ChangeDateBirth(DateTime date)
        {
            if (date > DateTime.Now) throw new ArgumentException("Человек не мог родиться в будущем!");
            
            DateBirth = date;
        }

        public void AddParent(Parent parent)
        {
            if (Parents.Contains(parent)) throw new ArgumentException("Данный родитель уже был добавлен!");
            if (Parents.Count == 2) throw new ArgumentException("Родителей не может быть больше двух");

            Parents.Add(parent);
        }
    }
}
