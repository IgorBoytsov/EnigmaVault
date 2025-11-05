namespace EnigmaVault.PasswordService.Application.Features.Validators
{
    public interface IMayHasUserId
    {
        public Guid? UserId { get; }
    }
}