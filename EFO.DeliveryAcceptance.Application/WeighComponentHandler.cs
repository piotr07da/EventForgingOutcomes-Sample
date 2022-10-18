using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public sealed class WeighComponentHandler : IConsumer<WeighComponent>
{
    private readonly IRepository<Component> _componentRepository;
    private readonly IRepository<ComponentInspector> _componentInspectorRepository;

    public WeighComponentHandler(IRepository<Component> componentRepository, IRepository<ComponentInspector> componentInspectorRepository)
    {
        _componentRepository = componentRepository ?? throw new ArgumentNullException(nameof(componentRepository));
        _componentInspectorRepository = componentInspectorRepository ?? throw new ArgumentNullException(nameof(componentInspectorRepository));
    }

    public async Task Consume(ConsumeContext<WeighComponent> context)
    {
        var command = context.Message;

        var component = await _componentRepository.GetAsync(command.ComponentId);
        var componentInspector = await _componentInspectorRepository.GetAsync(command.ComponentInspectorId);

        component.Weigh(componentInspector, Weight.FromValue(command.Weight));

        await _componentRepository.SaveAsync(command.ComponentId, component, ExpectedVersion.Any, context);
    }
}
