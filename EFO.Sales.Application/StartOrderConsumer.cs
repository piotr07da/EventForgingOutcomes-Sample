using EFO.Sales.Domain;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public sealed class StartOrderConsumer : IConsumer<StartOrder>
{
    private readonly IRepository<Order> _orderRepository;

    public StartOrderConsumer(IRepository<Order> orderRepository)
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
