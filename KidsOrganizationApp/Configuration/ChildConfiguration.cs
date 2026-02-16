using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Configuration
{
    public class ChildConfiguration : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            builder.ToTable("Children");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.FullName.Surname)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.FullName.Patronymic)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.DateBirth)
                   .IsRequired();

            // --- Many-to-Many с Parent ---
            builder
                .HasMany(c => c.Parents)
                .WithMany(p => p.Children)
                .UsingEntity<Dictionary<string, object>>(
                    "ParentChildren",
                    j => j.HasOne<Parent>().WithMany().HasForeignKey("ParentId"),
                    j => j.HasOne<Child>().WithMany().HasForeignKey("ChildId"),
                    j => j.ToTable("ParentChildren")
                );

            // --- Owned Object: PersonInfo ---
            builder.OwnsOne(c => c.Info, info =>
            {
                // Внутри PersonInfo есть Contact — тоже Owned
                info.OwnsOne(i => i.Contact, contact =>
                {
                    contact.Property(c => c.MobileNumber)
                           .HasMaxLength(15)
                           .IsRequired();

                    contact.Property(c => c.LivingPlace)
                           .HasMaxLength(200)
                           .IsRequired();
                });

                // Связь с Document
                info.HasOne(i => i.Passport)
                    .WithMany()
                    .HasForeignKey("PassportId");

                info.HasOne(i => i.SNILS)
                    .WithMany()
                    .HasForeignKey("SNILSId");

                info.HasOne(i => i.DiagnosisFile)
                    .WithMany()
                    .HasForeignKey("DiagnosisFileId");

                info.Property(i => i.MembershipStatus)
                    .IsRequired();
            });
        }
    }
}
