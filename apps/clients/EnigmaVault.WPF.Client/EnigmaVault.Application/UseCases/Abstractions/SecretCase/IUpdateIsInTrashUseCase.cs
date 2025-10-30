using EnigmaVault.Domain.Results;

namespace EnigmaVault.Application.UseCases.Abstractions.SecretCase
{
    public interface IUpdateIsInTrashUseCase
    {
        Task<Result> UpdateIsInTrashAsync(int idSecret, bool isInTrash);
    }
}