using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Configuration
{
    public class PersonInfoConfiguration : IEntityTypeConfiguration<PersonInfo>
    {
        public void Configure(EntityTypeBuilder<PersonInfo> builder)
        {
            builder.ToTable("PersonInfos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.MembershipStatus)
                   .IsRequired()
                   .HasMaxLength(50);

            builder
                .HasOne(p => p.Contact)
                .WithOne()
                .HasForeignKey<PersonInfo>(p => p.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Passport)
                .WithOne()
                .HasForeignKey<PersonInfo>(p => p.PassportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.SNILS)
                .WithOne()
                .HasForeignKey<PersonInfo>(p => p.SNILSId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.DiagnosisFile)
                .WithOne()
                .HasForeignKey<PersonInfo>(p => p.DiagnosisFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
