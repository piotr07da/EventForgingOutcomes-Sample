using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public sealed class CompleteComponentInspectionHandler : IConsumer<CompleteComponentInspection>
{
    private readonly IRepository<Component> _componentRepository;
    private readonly IRepository<ComponentInspector> _componentInspectorRepository;

    public CompleteComponentInspectionHandler(IRepository<Component> componentRepository, IRepository<ComponentInspector> componentInspectorRepository)
    {
        _componentRepository = componentRepository ?? throw new ArgumentNullException(nameof(componentRepository));
        _componentInspectorRepository = componentInspectorRepository ?? throw new ArgumentNullException(nameof(componentInspectorRepository));
    }

    public async Task Consume(ConsumeContext<CompleteComponentInspection> context)
    {
        var command = context.Message;

        var component = await _componentRepository.GetAsync(command.ComponentId);
        var componentInspector = await _componentInspectorRepository.GetAsync(command.ComponentInspectorId);

        component.CompleteInspection(componentInspector);

        await _componentRepository.SaveAsync(command.ComponentId, component, ExpectedVersion.Any, context);
    }
}
