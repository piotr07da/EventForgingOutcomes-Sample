using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public class HireComponentInspectorHandler : IConsumer<HireComponentInspector>
{
    private readonly IRepository<ComponentInspector> _componentInspectorRepository;

    public HireComponentInspectorHandler(IRepository<ComponentInspector> componentInspectorRepository)
    {
        _componentInspectorRepository = componentInspectorRepository ?? throw new ArgumentNullException(nameof(componentInspectorRepository));
    }

    public async Task Consume(ConsumeContext<HireComponentInspector> context)
    {
        var command = context.Message;

        var inspector = ComponentInspector.Hire(ComponentInspectorId.FromValue(command.InspectorId));

        await _componentInspectorRepository.SaveAsync(command.InspectorId, inspector, ExpectedVersion.None, context);
    }
}
