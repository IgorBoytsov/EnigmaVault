using EnigmaVault.SecretService.Domain.Results;
using MediatR;

namespace EnigmaVault.SecretService.Application.Features.Secrets.ManagementTrash
{
    public record RestoreItemFromTrashCommand(int SecretId) : IRequest<Result<DateTime>>;
}