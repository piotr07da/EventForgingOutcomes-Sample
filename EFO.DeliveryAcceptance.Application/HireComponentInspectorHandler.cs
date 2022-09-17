using EFO.DeliveryAcceptance.Domain;
using EventForging;
using MassTransit;

namespace EFO.DeliveryAcceptance.Application;

public class HireComponentInspectorHandler : IConsumer<HireComponentInspector>
{
    private readonly IRepository<ComponentInspector> _repository;

    public HireComponentInspectorHandler(IRepository<ComponentInspector> repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<HireComponentInspector> context)
    {
        var command = context.Message;

        var inspector = ComponentInspector.Hire(ComponentInspectorId.FromValue(command.InspectorId), command.CertificationLevel);

        await _repository.SaveAsync(command.InspectorId, inspector, ExpectedVersion.None, context);
    }
}
