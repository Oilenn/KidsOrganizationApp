using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Configuration
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable("Parents");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedNever();

            builder.Property(p => p.DateBirth)
                   .IsRequired();

            builder.Property(p => p.MembershipStatus)
                   .HasConversion<int>()
                   .IsRequired();

            // --- FullName (Value Object)
            builder.OwnsOne(p => p.FullName, fn =>
            {
                fn.Property(f => f.Name)
                  .HasMaxLength(100)
                  .IsRequired();

                fn.Property(f => f.Surname)
                  .HasMaxLength(100)
                  .IsRequired();

                fn.Property(f => f.Patronymic)
                  .HasMaxLength(100);
            });

            // --- Contact (Value Object)
            builder.OwnsOne(p => p.Contact, contact =>
            {
                contact.Property(c => c.MobileNumber)
                       .HasMaxLength(11)
                       .IsRequired();

                contact.Property(c => c.LivingPlace)
                       .HasMaxLength(200)
                       .IsRequired();

                contact.Property(c => c.Email)
                       .HasMaxLength(150);
            });

            // --- Documents (1:N)
            builder.HasMany(p => p.Documents)
                   .WithOne()
                   .HasForeignKey("ParentId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
