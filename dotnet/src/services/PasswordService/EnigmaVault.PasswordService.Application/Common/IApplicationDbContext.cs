using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.PasswordService.Application.Common
{
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}