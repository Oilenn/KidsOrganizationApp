using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .ValueGeneratedNever();

            builder.Property(e => e.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(e => e.Date)
                   .IsRequired();

            builder.HasMany(e => e.Documents)
                   .WithOne()
                   .HasForeignKey("EventId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
