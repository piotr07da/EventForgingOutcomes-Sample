using EFO.Sales.Application.ReadModel;
using MassTransit;

namespace EFO.Sales.Application.Queries;

public sealed class GetOrderHandler : IConsumer<GetOrder>
{
    private readonly IOrdersReadModel _readModel;

    public GetOrderHandler(IOrdersReadModel readModel)
    {
        _readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
    }

    public async Task Consume(ConsumeContext<GetOrder> context)
    {
        var query = context.Message;
        var order = _readModel.Get(query.OrderId);
        await context.RespondAsync(order);
    }
}
