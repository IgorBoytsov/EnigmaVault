using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnigmaVault.PasswordService.Infrastructure.Persistence.Contexts
{
    public sealed class EnigmaContext(DbContextOptions<EnigmaContext> options) : DbContext(options), IApplicationDbContext, IUnitOfWork
    {
        public DbSet<Icon> Icon { get; set; }
        public DbSet<IconCategory> IconCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}