using EnigmaVault.PasswordService.Domain.Models;
using EnigmaVault.PasswordService.Domain.ValueObjects.Password;
using EnigmaVault.PasswordService.Domain.ValueObjects.SvgIcon;
using EnigmaVault.PasswordService.Domain.ValueObjects.User;
using EnigmaVault.PasswordService.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnigmaVault.PasswordService.Infrastructure.Persistence.Configurations
{
    internal sealed class IconConfiguration : IEntityTypeConfiguration<Icon>
    {
        public void Configure(EntityTypeBuilder<Icon> builder)
        {
            builder.ToTable("Icons");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("id")
                .HasConversion(iconId => iconId.Value, dbValue => IconId.Create(dbValue))
                .ValueGeneratedNever();

            builder.Property(i => i.UserId)
                .HasColumnName("UserId")
                .HasConversion(new ValueConverter<UserId, Guid>(v => v.Value, v => UserId.Create(v)))
                .IsRequired(false);

            builder.Property(i => i.SvgCode)
                .HasColumnName("SvgCode")
                .HasConversion(code => code.Value, dbValue => SvgCode.Create(dbValue))
                .IsRequired(true);

            builder.Property(i => i.IconName)
                .HasColumnName("IconName")
                .HasConversion(name => name.Value, dbValue => IconName.Create(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME);

            builder.Property(i => i.IconCategoryId)
                .HasColumnName("IconCategoryId")
                .IsRequired(true);

            builder.HasOne<IconCategory>()
                .WithMany().HasForeignKey(i => i.IconCategoryId).HasPrincipalKey(ic => ic.Id)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}