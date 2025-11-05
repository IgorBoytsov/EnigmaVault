namespace EnigmaVault.PasswordService.Application.Features.Validators
{
    public interface IMustHasUserId
    {
        public Guid UserId { get; }
    }
}