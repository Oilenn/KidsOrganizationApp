using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .ValueGeneratedNever();

            builder.Property(c => c.MobileNumber)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(c => c.LivingPlace)
                   .IsRequired()
                   .HasMaxLength(255);
        }
    }
}
