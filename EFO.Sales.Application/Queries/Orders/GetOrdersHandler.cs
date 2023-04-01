using EFO.Sales.Application.ReadModel.Orders;
using MassTransit;

namespace EFO.Sales.Application.Queries.Orders;

public sealed class GetOrdersHandler : IConsumer<GetOrders>
{
    private readonly IOrdersReadModel _readModel;

    public GetOrdersHandler(IOrdersReadModel readModel)
    {
        _readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
    }

    public async Task Consume(ConsumeContext<GetOrders> context)
    {
        var orders = _readModel.GetAll();
        await context.RespondAsync(orders);
    }
}
