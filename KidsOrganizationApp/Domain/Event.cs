using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Event
    {
        public Guid Id { get; private set; }    
        public string Name { get; private set; } = string.Empty;
        public DateTime Date { get; private set; } = DateTime.MinValue;
        public List<Document> Documents { get; private set; } = [];

        private const int NameLenght = 100;

        protected Event() { }

        public Event(string name, DateTime dateTime, List<Document> documents)
        {
            ChangeName(name);
            ChangeDate(dateTime);
            AddDocument(documents);

            Id = Guid.NewGuid();
        }

        public Event(string name, DateTime dateTime) : this(name, dateTime, []) { }

        public void ChangeDate(DateTime dateTime)
        {
            Date = dateTime;
        }

        public void AddDocument(List<Document> documents)
        {
            foreach (Document document in documents)
            {
                Documents.Add(document);
            }
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым!");

            name = name.Trim();

            if (name.Length > NameLenght)
                name = name.Substring(0, NameLenght);

            Name = name;
        }

    }
}
