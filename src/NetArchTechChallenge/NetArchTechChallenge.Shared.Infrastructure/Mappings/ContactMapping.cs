using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetArchTechChallenge.Shared.Domain.Entities;

namespace NetArchTechChallenge.Shared.Infrastructure.Mappings
{
    public class ContactMapping : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100);

            builder.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasColumnName("PhoneNumber")
                .HasColumnType("VARCHAR(9)")
                .HasMaxLength(9);

            builder.Property(t => t.PhoneDDD)
                .IsRequired()
                .HasColumnName("PhoneDDD")
                .HasColumnType("VARCHAR(2)")
                .HasMaxLength(2);

            builder.Property(e => e.EmailAddress)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100);
        }
    }
}
