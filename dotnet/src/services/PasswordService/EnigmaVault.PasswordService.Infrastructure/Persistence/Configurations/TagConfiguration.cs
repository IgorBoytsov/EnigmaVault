using EnigmaVault.PasswordService.Domain.Models;
using EnigmaVault.PasswordService.Domain.ValueObjects.Common;
using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using EnigmaVault.PasswordService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnigmaVault.PasswordService.Infrastructure.Persistence.Configurations
{
    internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasConversion(tagId => tagId.Value, dbValue => TagId.Create(dbValue))
                .ValueGeneratedNever();

            builder.Property(t => t.UserId)
                .HasColumnName("UserId")
                .HasConversion(userId => userId.Value, dbValue => UserId.Create(dbValue))
                .IsRequired(true);

            builder.Property(t => t.Name)
                .HasColumnName("IconName")
                .HasConversion(name => name.Value, dbValue => TagName.Create(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME);

            builder.Property(f => f.Color)
                .HasColumnName("Color")
                .HasConversion(color => color.Value, dbValue => Color.FromHex(dbValue))
                .IsRequired(true);
        }
    }
}