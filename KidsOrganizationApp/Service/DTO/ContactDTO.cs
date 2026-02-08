using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service.DTO
{
    public class ContactDTO : IDTO
    {
        public Guid Id { get; set; }
        public string MobileNumber { get; set; }
        public string LivingPlace { get; set; }
    }

}
