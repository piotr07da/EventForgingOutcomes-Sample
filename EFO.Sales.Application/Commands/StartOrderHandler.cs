using EFO.Sales.Domain;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands;

public sealed class StartOrderHandler : IConsumer<StartOrder>
{
    private readonly IRepository<Order> _orderRepository;

    public StartOrderHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task Consume(ConsumeContext<StartOrder> context)
    {
        var command = context.Message;
        var order = Order.Start(command.OrderId, command.CustomerId);
        await _orderRepository.SaveAsync(command.OrderId, order, context);
    }
}
