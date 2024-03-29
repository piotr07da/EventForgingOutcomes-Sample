﻿using EFO.Sales.Domain.Orders;
using EFO.Sales.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands.Orders;

public sealed class ChangeOrderItemQuantityHandler : IConsumer<ChangeOrderItemQuantity>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;

    public ChangeOrderItemQuantityHandler(IRepository<Order> orderRepository, IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<ChangeOrderItemQuantity> context)
    {
        var command = context.Message;

        var order = await _orderRepository.GetAsync(command.OrderId, context);
        var product = await _productRepository.GetAsync(order.Items.Find(command.OrderItemId).ProductId, context);

        order.ChangeItemQuantity(command.OrderItemId, product, command.Quantity);

        await _orderRepository.SaveAsync(command.OrderId, order, context);
    }
}
