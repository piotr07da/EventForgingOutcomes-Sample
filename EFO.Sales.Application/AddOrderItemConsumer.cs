using EFO.Sales.Domain;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application;

public sealed class AddOrderItemConsumer : IConsumer<AddOrderItem>
{
    private readonly IRepository<Order> _ordererRepository;
    private readonly IRepository<Product> _productRepository;

    public AddOrderItemConsumer(IRepository<Order> ordererRepository, IRepository<Product> productRepository)
    {
        _ordererRepository = ordererRepository ?? throw new ArgumentNullException(nameof(ordererRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<AddOrderItem> context)
    {
        var command = context.Message;

        var order = await _ordererRepository.GetAsync(command.OrderId, context);
        var product = await _productRepository.GetAsync(command.ProductId, context);
        order.AddItem(command.OrderItemId, product, command.Quantity);
        await _ordererRepository.SaveAsync(command.OrderId, order, context);
    }
}
