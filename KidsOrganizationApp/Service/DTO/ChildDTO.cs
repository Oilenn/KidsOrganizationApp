using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    //todo!!!!: разделить на несколько Dto
    public class ChildDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string MobileNumber { get; set; }
        public string LivingPlace { get; set; }
        public string Email { get; set; }
        public DateTime DateBirth { get; set; }

        public int MembershipStatus { get; set; }

        public List<Guid> ParentIds { get; set; } = new List<Guid>();
        public List<Guid> DocumentIds { get; set; } = new List<Guid>();

        public ChildDTO(
            Guid id,
            string name,
            string surname,
            string patronymic,
            string mobileNumber,
            string livingPlace,
            DateTime dateBirth,
            string email = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            Patronymic = patronymic;
            MobileNumber = mobileNumber ?? throw new ArgumentNullException(nameof(mobileNumber));
            LivingPlace = livingPlace ?? throw new ArgumentNullException(nameof(livingPlace));
            DateBirth = dateBirth;
            Email = email;
        }

        public ChildDTO(
            string name,
            string surname,
            string patronymic,
            string mobileNumber,
            string livingPlace,
            DateTime dateBirth,
            string email = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            Patronymic = patronymic;
            MobileNumber = mobileNumber ?? throw new ArgumentNullException(nameof(mobileNumber));
            LivingPlace = livingPlace ?? throw new ArgumentNullException(nameof(livingPlace));
            DateBirth = dateBirth;
            Email = email;
        }
    }
}
