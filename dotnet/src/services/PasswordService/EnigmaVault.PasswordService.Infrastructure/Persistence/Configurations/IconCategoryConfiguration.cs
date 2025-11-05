using EnigmaVault.PasswordService.Domain.Models;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIconCategory;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using EnigmaVault.PasswordService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnigmaVault.PasswordService.Infrastructure.Persistence.Configurations
{
    internal sealed class IconCategoryConfiguration : IEntityTypeConfiguration<IconCategory>
    {
        public void Configure(EntityTypeBuilder<IconCategory> builder)
        {
            builder.ToTable("IconCategories");
            builder.HasKey(ic => ic.Id);

            builder.Property(ic => ic.Id)
                .HasColumnName("Id")
                .HasConversion(iconCatId => iconCatId.Value, dbValue => IconCategoryId.Create(dbValue))
                .ValueGeneratedNever();

            builder.Property(i => i.UserId)
                .HasColumnName("UserId")
                .HasConversion(new ValueConverter<UserId, Guid>(v => v.Value, v => UserId.Create(v)))
                .IsRequired(false);

            builder.Property(ic => ic.Name)
                .HasColumnName("Name")
                .HasConversion(name => name.Value, dbValue => IconCategoryName.Create(dbValue))
                .HasMaxLength(IconCategoryName.MAX_LENGTH)
                .IsRequired(true)
                .UseCollation(PostgresConstants.COLLATION_NAME);
        }
    }
}