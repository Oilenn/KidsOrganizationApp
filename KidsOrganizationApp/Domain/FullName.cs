using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    //Value Object
    public class FullName : IEquatable<FullName>
    {
        public string Name { get; }
        public string Surname { get; }
        public string? Patronymic { get; }

        protected FullName() { }

        public FullName(string name, string surname, string? patronymic)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым");

            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Фамилия не может быть пустой");

            Name = name.Trim();
            Surname = surname.Trim();
            Patronymic = string.IsNullOrWhiteSpace(patronymic)
                ? null
                : patronymic.Trim();
        }

        public bool Equals(FullName? other)
        {
            if (other is null) return false;

            return Name == other.Name &&
                   Surname == other.Surname &&
                   Patronymic == other.Patronymic;
        }

        public override bool Equals(object? obj)
            => Equals(obj as FullName);

        public override int GetHashCode()
            => HashCode.Combine(Name, Surname, Patronymic);

        public override string ToString()
            => Patronymic is null
                ? $"{Surname} {Name}"
                : $"{Surname} {Name} {Patronymic}";
    }
}
