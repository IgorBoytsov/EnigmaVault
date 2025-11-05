using Common.Core.Primitives;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.PasswordService.Infrastructure.Repositories
{
    internal sealed class IconCategoryRepository(IApplicationDbContext context) : IIconCategoryRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(IconCategory iconCategory, CancellationToken token) => await _context.Set<IconCategory>().AddAsync(iconCategory, token);

        public void Remove(IconCategory iconCategory) => _context.Set<IconCategory>().Remove(iconCategory);

        public async Task<Maybe<IconCategory>> GetAsync(Guid id, Guid? UserId = null, CancellationToken token = default)
        {
            if (UserId != null)
                return await _context.Set<IconCategory>().FirstOrDefaultAsync(ic => ic.Id == id && ic.UserId == UserId, token);

            return await _context.Set<IconCategory>().FirstOrDefaultAsync(ic => ic.Id == id, token);
        }
    }
}