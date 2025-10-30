using EnigmaVault.SecretService.Domain.Results;
using MediatR;

namespace EnigmaVault.SecretService.Application.Features.Secrets.ManagementTrash
{
    public record MoveSecretToTrashCommand(int SecretId) : IRequest<Result<DateTime>>;
}