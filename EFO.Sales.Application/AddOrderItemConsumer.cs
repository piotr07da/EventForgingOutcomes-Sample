using EFO.Sales.Domain;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public sealed class AddOrderItemConsumer : IConsumer<AddOrderItem>
{
    private readonly IRepository<Order> _ordererRepository;

    public AddOrderItemConsumer(IRepository<Order> ordererRepository)
    {
        _ordererRepository = ordererRepository ?? throw new ArgumentNullException(nameof(ordererRepository));
    }

    public async Task Consume(ConsumeContext<AddOrderItem> context)
    {
        var command = context.Message;

        var order = await _ordererRepository.GetAsync(command.OrderId, context);
        order.AddItem(command.OrderItemId, command.ProductId, command.Quantity);
        await _ordererRepository.SaveAsync(command.OrderId, order, context);
    }
}
