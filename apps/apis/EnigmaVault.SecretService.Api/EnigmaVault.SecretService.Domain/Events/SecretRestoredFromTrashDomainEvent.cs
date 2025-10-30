namespace EnigmaVault.SecretService.Domain.Events
{
    public record SecretRestoredFromTrashDomainEvent(int SecretId) : DomainEvent;
}