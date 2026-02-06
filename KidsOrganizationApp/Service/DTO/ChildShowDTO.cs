namespace KidsOrganizationApp.Service.DTO
{
    public class ChildShowDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; } = DateTime.MinValue;
        public List<ParentDTO> Parents { get; set; } = [];

        public ChildShowDTO() { }
        public ChildShowDTO(string name, string surname, string patronymic, DateTime dateBirth, List<ParentDTO> parents)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            DateBirth = dateBirth;
            Parents = parents;
        }
    }
}