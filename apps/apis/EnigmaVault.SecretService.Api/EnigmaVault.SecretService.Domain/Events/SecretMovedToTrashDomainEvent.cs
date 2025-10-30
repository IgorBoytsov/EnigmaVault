namespace EnigmaVault.SecretService.Domain.Events
{
    public record SecretMovedToTrashDomainEvent(int SecretId) : DomainEvent;
}