using Common.Core.Primitives;
using EnigmaVault.PasswordService.Domain.Models;

namespace EnigmaVault.PasswordService.Application.Common.Repositories
{
    public interface IIconCategoryRepository
    {
        Task AddAsync(IconCategory iconCategory, CancellationToken token);
        Task<Maybe<IconCategory>> GetAsync(Guid id, Guid? UserId = null, CancellationToken token = default);
        void Remove(IconCategory iconCategory);
    }
}