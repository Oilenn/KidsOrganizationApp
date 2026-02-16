using System;
using System.Linq;

namespace KidsOrganizationApp.Domain
{
    // Value Object
    public class Contact : IEquatable<Contact>
    {
        public string MobileNumber { get; }
        public string LivingPlace { get; }
        public string? Email { get; }

        private const int MaxLengthNumber = 11;

        protected Contact() { }

        public Contact(string mobileNumber, string livingPlace, string? email = null)
        {
            MobileNumber = NormalizePhone(mobileNumber);
            LivingPlace = NormalizeLivingPlace(livingPlace);
            Email = NormalizeEmail(email);
        }

        private static string NormalizePhone(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
                throw new ArgumentException("Номер телефона обязателен");

            var normalized = mobileNumber.Trim();

            if (normalized.Length != MaxLengthNumber)
                throw new ArgumentException("Номер телефона должен содержать 11 цифр");

            if (!normalized.All(char.IsDigit))
                throw new ArgumentException("Номер телефона должен содержать только цифры");

            return normalized;
        }

        private static string NormalizeLivingPlace(string livingPlace)
        {
            if (string.IsNullOrWhiteSpace(livingPlace))
                throw new ArgumentException("Адрес обязателен");

            return livingPlace.Trim();
        }

        private static string? NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var normalized = email.Trim();

            if (!normalized.Contains("@") || normalized.StartsWith("@") || normalized.EndsWith("@"))
                throw new ArgumentException("Некорректный email");

            return normalized;
        }

        public bool Equals(Contact? other)
        {
            if (other is null) return false;

            return MobileNumber == other.MobileNumber &&
                   LivingPlace == other.LivingPlace &&
                   Email == other.Email;
        }

        public override bool Equals(object? obj)
            => Equals(obj as Contact);

        public override int GetHashCode()
            => HashCode.Combine(MobileNumber, LivingPlace, Email);

        public override string ToString()
            => Email is null
                ? $"{MobileNumber}, {LivingPlace}"
                : $"{MobileNumber}, {LivingPlace}, {Email}";
    }
}
