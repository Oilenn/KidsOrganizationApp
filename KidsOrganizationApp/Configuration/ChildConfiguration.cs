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
    public class ChildConfiguration : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            builder.ToTable("Child");

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
                .HasMany(p => p.Parents)
                .WithMany(c => c.Children)
                .UsingEntity(j =>
                {
                    j.ToTable("ParentChildren");
                });
        }
    }
}
