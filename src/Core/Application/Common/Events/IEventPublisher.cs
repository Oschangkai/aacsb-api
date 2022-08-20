using AACSB.WebApi.Shared.Events;

namespace AACSB.WebApi.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}