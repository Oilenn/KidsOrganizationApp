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

        public FullName FullName { get; private set; }
        public Contact Contact { get; private set; }

        public DateTime DateBirth { get; private set; } = DateTime.MinValue;

        public MembershipStatus MembershipStatus { get; private set; } = MembershipStatus.Active;

        public List<Parent> Parents { get; private set; } = new List<Parent>();
        public List<Document> Documents = new List<Document>();

        private const int MaxParents = 2;

        protected Child() { }

        public Child(FullName fullName,
              Contact contact,
              DateTime dateBirth,
              List<Parent> parents,
              List<Document> documents)
        {
            Contact = contact;
            FullName = fullName;

            ChangeDateBirth(dateBirth);
            AddParents(parents);

            Id = Guid.NewGuid();
        }

        public void ChangeDateBirth(DateTime dateBirth)
        {
            if (dateBirth > DateTime.Now) 
                throw new ArgumentException("Дата рождения не может превышать сегодняшний день!");

            DateBirth = dateBirth;
        }

        public void AddParents(List<Parent> parents)
        {
            foreach (Parent parent in parents)
            {
                Parents.Add(parent);
            }
        }

        public void AddParent(Parent parent)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            if (Parents.Count >= MaxParents)
                throw new InvalidOperationException("У ребенка не может быть больше 2 родителей.");

            if (Parents.Any(p => p.Id == parent.Id))
                throw new InvalidOperationException("Этот родитель уже добавлен.");

            Parents.Add(parent);
        }
    }

    public enum MembershipStatus
    {
        Active = 0,
        Left = 1
    }
}
