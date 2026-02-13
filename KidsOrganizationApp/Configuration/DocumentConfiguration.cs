using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidsOrganizationApp.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                   .ValueGeneratedNever();

            // Enum хранится как int (по умолчанию)
            builder.Property(d => d.Type)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(d => d.Path)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
