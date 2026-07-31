using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Configuration
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                   .ValueGeneratedNever();

            builder.Property(d => d.Type)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(d => d.Path)
                   .HasMaxLength(500)
                   .IsRequired();
        }
    }
}
