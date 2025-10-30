using MediatR;

namespace EnigmaVault.SecretService.Domain
{
    public abstract record DomainEvent : INotification;
}