using KidsOrganizationApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Configuration
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable("Parents");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Surname)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Patronymic)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.DateBirth)
                   .IsRequired();

            builder
                .HasMany(p => p.Children)
                .WithMany(c => c.Parents)
                .UsingEntity(j =>
                {
                    j.ToTable("ParentChildren");
                });
        }
    }
}
