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

            builder.Property(c => c.Id)
                   .ValueGeneratedNever();

            builder.Property(c => c.DateBirth)
                   .IsRequired();

            builder.Property(c => c.MembershipStatus)
                   .HasConversion<int>()
                   .IsRequired();

            // --- FullName (VO)
            builder.OwnsOne(c => c.FullName, fn =>
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

            // --- Contact (VO)
            builder.OwnsOne(c => c.Contact, contact =>
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

            // --- Many-to-Many Parent
            builder
                .HasMany(c => c.Parents)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ParentChildren",
                    j => j.HasOne<Parent>().WithMany().HasForeignKey("ParentId"),
                    j => j.HasOne<Child>().WithMany().HasForeignKey("ChildId"),
                    j =>
                    {
                        j.HasKey("ParentId", "ChildId");
                        j.ToTable("ParentChildren");
                    });

            // --- Documents (1:N)
            builder.HasMany(c => c.Documents)
                   .WithOne()
                   .HasForeignKey("ChildId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
