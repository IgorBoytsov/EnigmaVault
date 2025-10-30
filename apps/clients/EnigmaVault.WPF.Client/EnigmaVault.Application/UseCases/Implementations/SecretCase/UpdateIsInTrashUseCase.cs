using EnigmaVault.Application.Abstractions.Repositories;
using EnigmaVault.Application.UseCases.Abstractions.SecretCase;
using EnigmaVault.Domain.Results;

namespace EnigmaVault.Application.UseCases.Implementations.SecretCase
{
    public sealed class UpdateIsInTrashUseCase(ISecretRepository secretRepository) : IUpdateIsInTrashUseCase
    {
        private readonly ISecretRepository _secretRepository = secretRepository;

        public async Task<Result> UpdateIsInTrashAsync(int idSecret, bool isInTrash) => await _secretRepository.UpdateIsInTrashAsync(idSecret, isInTrash);
    }
}