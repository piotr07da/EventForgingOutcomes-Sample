using EFO.Catalog.Domain.Products;
using EventForging.EventsHandling;
using Microsoft.Extensions.Logging;

namespace EFO.Catalog.Application.EventHandling;

public sealed class CatalogEventHandlers :
    IEventHandler<ProductIntroduced>,
    IEventHandler<ProductNamed>,
    IEventHandler<ProductMovedToCategory>,
    IEventHandler<ProductNumericPropertySet>,
    IEventHandler<ProductTextPropertySet>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger _logger;

    public CatalogEventHandlers(IEventDispatcher eventDispatcher, ILoggerFactory loggerFactory)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _logger = loggerFactory.CreateLogger("EFO.Catalog.Application");
    }

    public string SubscriptionName => "MainPipeline";

    public async Task HandleAsync(ProductIntroduced e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductNamed e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductMovedToCategory e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductNumericPropertySet e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

    public async Task HandleAsync(ProductTextPropertySet e, EventInfo ei, CancellationToken cancellationToken) => await DispatchAsync(e, ei, cancellationToken);

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
