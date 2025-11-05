using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal
{
    internal sealed class GetAllPersonalIconCategoriesHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAllPersonalIconCategories, List<IconCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<IconCategoryResponse>> Handle(GetAllPersonalIconCategories request, CancellationToken cancellationToken)
            => await _context.Set<IconCategory>()
                .Where(ic => ic.UserId == request.UserId)
                    .ProjectTo<IconCategoryResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}