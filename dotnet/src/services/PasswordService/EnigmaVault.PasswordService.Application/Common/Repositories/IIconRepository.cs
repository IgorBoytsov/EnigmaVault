using Common.Core.Primitives;
using EnigmaVault.PasswordService.Domain.Models;

namespace EnigmaVault.PasswordService.Application.Common.Repositories
{
    public interface IIconRepository
    {
        Task AddAsync(Icon icon, CancellationToken token);
        Task<Maybe<Icon>> GetAsync(Guid id, Guid? UserId = null, CancellationToken token = default);
        void Remove(Icon icon);
    }
}