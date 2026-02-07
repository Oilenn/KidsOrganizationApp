using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Domain
{
    public class Contact
    {
        public Guid Id { get; private set; }
        public string MobileNumber { get; private set; } = string.Empty;
        public string LivingPlace { get; private set; } = string.Empty;

        private const int _maxLenghtNumber = 11;

        protected Contact() { }

        public Contact(string mobileNumber, string livingPlace)
        {
            ChangeMobileNumber(mobileNumber);
            ChangeLivingPlace(livingPlace);

            Id = Guid.NewGuid();
        }

        public void ChangeMobileNumber(string mobileNumber)
        {
            MobileNumber = mobileNumber.Trim().Remove(_maxLenghtNumber - 1);
        }

        public void ChangeLivingPlace(string livingPlace)
        {
            LivingPlace = livingPlace;
        }
    }
}
