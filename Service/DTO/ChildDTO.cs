namespace KidsOrganizationApp.Service.DTO;

public class ChildDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string MobileNumber { get; set; }
    public string LivingPlace { get; set; }
    public string? Email { get; set; }
    public DateTime DateBirth { get; set; }
    public int MembershipStatus { get; set; }
    public List<Guid> ParentIds { get; set; } = new();
    public List<Guid> DocumentIds { get; set; } = new();
    public ChildDTO(Guid id, string name, string surname, string patronymic, string mobileNumber, string livingPlace, DateTime dateBirth, List<Guid> parents, string? email = null)
    { Id = id; Name = name; Surname = surname; Patronymic = patronymic; MobileNumber = mobileNumber; LivingPlace = livingPlace; DateBirth = dateBirth; ParentIds = parents; Email = email; }
    public ChildDTO(string name, string surname, string patronymic, string mobileNumber, string livingPlace, DateTime dateBirth, List<Guid> parents, string? email = null)
        : this(Guid.Empty, name, surname, patronymic, mobileNumber, livingPlace, dateBirth, parents, email) { }
}
