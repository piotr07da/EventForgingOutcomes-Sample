using MassTransit;

namespace EFO.Sales.Application.Queries;

public sealed record GetOrder(Guid OrderId);

internal sealed class GetOrderHandler : IConsumer<GetOrder>
{
    private readonly IOrdersReadModel _readModel;

    public GetOrderHandler(IOrdersReadModel readModel)
    {
        _readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
    }

    public async Task Consume(ConsumeContext<GetOrder> context)
    {
        var query = context.Message;
        var order = _readModel.GetOrAdd(query.OrderId);
        await context.RespondAsync(order);
    }
}

public sealed record OrderDto(Guid OrderId)
{
    public OrderDto()
        : this(Guid.Empty)
    {
    }

    public Guid CustomerId { get; set; }
}

internal interface IOrdersReadModel
{
    OrderDto GetOrAdd(Guid orderId);
}

internal sealed class OrdersReadModel : IOrdersReadModel
{
    private readonly IDictionary<Guid, OrderDto> _entries = new Dictionary<Guid, OrderDto>();

    public OrderDto GetOrAdd(Guid orderId)
    {
        if (!_entries.TryGetValue(orderId, out var order))
        {
            order = new OrderDto(orderId);
            _entries.Add(orderId, order);
        }

        return order;
    }
}
