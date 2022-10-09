using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public record CompleteComponentInspection(Guid ComponentId, Guid ComponentInspectorId);

public sealed class CompleteComponentInspectionHandler : IConsumer<CompleteComponentInspection>
{
    private readonly IRepository<Component> _componentRepository;
    private readonly IRepository<ComponentInspector> _componentInspectorRepository;

    public CompleteComponentInspectionHandler(IRepository<Component> componentRepository, IRepository<ComponentInspector> componentInspectorRepository)
    {
        _componentRepository = componentRepository;
        _componentInspectorRepository = componentInspectorRepository;
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
