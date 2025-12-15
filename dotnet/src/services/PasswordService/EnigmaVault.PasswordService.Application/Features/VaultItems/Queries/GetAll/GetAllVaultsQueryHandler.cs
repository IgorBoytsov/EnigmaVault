using AutoMapper;
using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Queries.GetAll
{
    public sealed class GetAllVaultsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllVaultsQuery, Result<List<EncryptedVaultResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<EncryptedVaultResponse>>> Handle(GetAllVaultsQuery request, CancellationToken cancellationToken)
            => await _context.Set<VaultItem>().Where(v => v.UserId == request.UserId)
                .Select(x => new EncryptedVaultResponse(
                    x.Id.ToString(), 
                    x.PasswordType.ToString(), 
                    x.DateAdded, 
                    x.DateUpdated, 
                    x.DeletedAt, 
                    x.IsFavorite, 
                    x.IsArchive, 
                    x.IsInTrash, 
                    x.EncryptedOverview, 
                    x.EncryptedDetails))
                .ToListAsync(cancellationToken);
    }
}