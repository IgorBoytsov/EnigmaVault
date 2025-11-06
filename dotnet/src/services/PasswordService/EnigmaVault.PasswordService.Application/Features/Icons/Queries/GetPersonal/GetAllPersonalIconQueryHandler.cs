using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetPersonal
{
    public sealed class GetAllPersonalIconQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllPersonalIconQuery, List<IconResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<IconResponse>> Handle(GetAllPersonalIconQuery request, CancellationToken cancellationToken)
            => await _context.Set<Icon>()
                .Where(i => i.UserId == request.UserId)
                    .ProjectTo<IconResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}