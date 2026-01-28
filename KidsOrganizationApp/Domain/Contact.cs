using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Contact
    {
        public int Id { get; set; }
        public string MobileNumber { get; private set; } = string.Empty;
        public string LivingPlace { get; private set; } = string.Empty;
    }
}
