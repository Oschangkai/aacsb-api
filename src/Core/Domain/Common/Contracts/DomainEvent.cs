using AACSB.WebApi.Shared.Events;

namespace AACSB.WebApi.Domain.Common.Contracts;

public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}