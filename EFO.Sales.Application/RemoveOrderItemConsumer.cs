using EFO.Sales.Domain;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public sealed class RemoveOrderItemConsumer : IConsumer<RemoveOrderItem>
{
    private readonly IRepository<Order> _orderRepository;

    public RemoveOrderItemConsumer(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task Consume(ConsumeContext<RemoveOrderItem> context)
    {
        var command = context.Message;

        var order = await _orderRepository.GetAsync(command.OrderId, context);
        order.RemoveItem(command.OrderItemId);
        await _orderRepository.SaveAsync(command.OrderId, order, context);
    }
}
