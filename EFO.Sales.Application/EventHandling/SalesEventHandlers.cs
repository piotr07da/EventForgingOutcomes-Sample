using EFO.Sales.Domain.Orders;
using EFO.Sales.Domain.Products;
using EventForging.EventsHandling;
using Microsoft.Extensions.Logging;

namespace EFO.Sales.Application.EventHandling;

public sealed class SalesEventHandlers :
    IEventHandler<OrderStarted>,
    IEventHandler<OrderCustomerAssigned>,
    IEventHandler<OrderItemAdded>,
    IEventHandler<OrderItemQuantityChanged>,
    IEventHandler<ProductIntroduced>,
    IEventHandler<ProductNamed>,
    IEventHandler<ProductPriced>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger _logger;

    public SalesEventHandlers(IEventDispatcher eventDispatcher, ILoggerFactory loggerFactory)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _logger = loggerFactory.CreateLogger("EFO.Sales.Application");
    }

    public string SubscriptionName => "MainPipeline";

    public async Task HandleAsync(OrderStarted e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(OrderCustomerAssigned e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(OrderItemAdded e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(OrderItemQuantityChanged e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductIntroduced e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductNamed e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductPriced e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

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

        await _eventDispatcher.DispatchAsync("IntegrationEventsPublisher", e, ei, cancellationToken);
    }
}
