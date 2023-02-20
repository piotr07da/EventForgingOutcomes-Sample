using EFO.Sales.Domain;
using EventForging.EventsHandling;
using Microsoft.Extensions.Logging;

namespace EFO.Sales.Application.EventHandling;

public sealed class EventHandlers :
    IEventHandler<OrderStarted>,
    IEventHandler<OrderCustomerAssigned>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger _logger;

    public EventHandlers(IEventDispatcher eventDispatcher, ILoggerFactory loggerFactory)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _logger = loggerFactory.CreateLogger("EFO.Sales.Application");
    }

    public string SubscriptionName => "MainPipeline";

    public async Task Handle(OrderStarted e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task Handle(OrderCustomerAssigned e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    private async Task DispatchAsync(object e, EventInfo ei, CancellationToken cancellationToken)
    {
        try
        {
            await _eventDispatcher.DispatchAsync("ReadModelBuilder", e, ei, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        await _eventDispatcher.DispatchAsync("IntegrationEventsPublished", e, ei, cancellationToken);
    }
}
