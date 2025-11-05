namespace EnigmaVault.PasswordService.Application.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}