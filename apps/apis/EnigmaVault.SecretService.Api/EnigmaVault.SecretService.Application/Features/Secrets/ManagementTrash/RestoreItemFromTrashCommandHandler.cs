using EnigmaVault.SecretService.Application.Abstractions.Repositories;
using EnigmaVault.SecretService.Domain.Results;
using MediatR;

namespace EnigmaVault.SecretService.Application.Features.Secrets.ManagementTrash
{
    public class RestoreItemFromTrashCommandHandler(ISecretRepository secretRepository) : IRequestHandler<RestoreItemFromTrashCommand, Result<DateTime>>
    {
        private readonly ISecretRepository _secretRepository = secretRepository;

        public async Task<Result<DateTime>> Handle(RestoreItemFromTrashCommand request, CancellationToken cancellationToken)
        {
            var domain = await _secretRepository.GetByIdAsync(request.SecretId, cancellationToken);

            domain?.RestoreFromTrash();

            var result = await _secretRepository.UpdateAsync(domain);

            return result;
        }
    }
}