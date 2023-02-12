using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public sealed record RegisterComponentArrival(Guid ComponentId, string Name);

public sealed class RegisterComponentArrivalHandler : IConsumer<RegisterComponentArrival>
{
    private readonly IRepository<Component> _componentRepository;

    public RegisterComponentArrivalHandler(IRepository<Component> componentRepository)
    {
        _componentRepository = componentRepository ?? throw new ArgumentNullException(nameof(componentRepository));
    }

    public async Task Consume(ConsumeContext<RegisterComponentArrival> context)
    {
        var command = context.Message;

        var component = Component.RegisterArrival(ComponentId.FromValue(command.ComponentId), ComponentName.FromValue(command.Name));

        await _componentRepository.SaveAsync(command.ComponentId, component, ExpectedVersion.None, context);
    }
}
