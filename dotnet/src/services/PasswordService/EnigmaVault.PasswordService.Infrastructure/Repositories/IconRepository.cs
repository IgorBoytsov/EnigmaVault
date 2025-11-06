using Common.Core.Primitives;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.PasswordService.Infrastructure.Repositories
{
    internal sealed class IconRepository(IApplicationDbContext context) : IIconRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(Icon icon, CancellationToken token) => await _context.Set<Icon>().AddAsync(icon, token);

        public void Remove(Icon icon) => _context.Set<Icon>().Remove(icon);

        public async Task<Maybe<Icon>> GetAsync(Guid id, Guid? UserId = null, CancellationToken token = default)
        {
            if (UserId != null)
                return await _context.Set<Icon>().FirstOrDefaultAsync(ic => ic.Id == id && ic.UserId == UserId, token);

            return await _context.Set<Icon>().FirstOrDefaultAsync(ic => ic.Id == id, token);
        }
    }
}