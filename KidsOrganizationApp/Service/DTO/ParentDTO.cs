using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    //todo!!!!: разделить на несколько Dto
    public class ParentDTO : IDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;

        public string MobileNumber { get; set; }
        public string LivingPlace { get; set; }
        public string Email { get; set; }

        public int MembershipStatus { get; set; }
        public DateTime DateBirth { get; set; } = DateTime.MinValue;

        public List<Guid> DocumentIds { get; set; } = new List<Guid>();

        public ParentDTO() { }

        public ParentDTO(Guid id,
            string name,
            string surname,
            string patronymic,
            string mobileNumber,
            string livingPlace,
            DateTime dateBirth,
            string email = null)
        {
            Id = id;
            Name = name;
            Surname = surname;
            MobileNumber = mobileNumber;
            LivingPlace = livingPlace;
            Patronymic = patronymic ?? throw new ArgumentNullException(nameof(patronymic));
            DateBirth = dateBirth;
        }

        public ParentDTO(
            string name,
            string surname,
            string patronymic,
            string mobileNumber,
            string livingPlace,
            DateTime dateBirth,
            string email = null)
        {
            Name = name;
            Surname = surname;
            MobileNumber = mobileNumber;
            LivingPlace = livingPlace;
            Patronymic = patronymic ?? throw new ArgumentNullException(nameof(patronymic));
            DateBirth = dateBirth;
        }
    }
}
