using EFO.Sales.Application.ReadModel.Orders;
using EFO.Sales.Domain.Orders;
using EventForging.EventsHandling;

namespace EFO.Sales.Application.EventHandling;

internal class OrdersReadModelBuildingEventHandlers :
    IEventHandler<OrderStarted>,
    IEventHandler<OrderCustomerAssigned>,
    IEventHandler<OrderItemAdded>,
    IEventHandler<OrderItemQuantityChanged>
{
    private readonly IOrdersReadModel _ordersReadModel;

    public OrdersReadModelBuildingEventHandlers(IOrdersReadModel ordersReadModel)
    {
        _ordersReadModel = ordersReadModel ?? throw new ArgumentNullException(nameof(ordersReadModel));
    }

    public string SubscriptionName => "ReadModelBuilder";

    public Task HandleAsync(OrderStarted e, EventInfo ei, CancellationToken cancellationToken)
    {
        _ordersReadModel.GetOrAdd(e.OrderId);
        return Task.CompletedTask;
    }

    public Task HandleAsync(OrderCustomerAssigned e, EventInfo ei, CancellationToken cancellationToken)
    {
        var order = _ordersReadModel.GetOrAdd(e.OrderId);
        order.CustomerId = e.CustomerId;
        return Task.CompletedTask;
    }

    public Task HandleAsync(OrderItemAdded e, EventInfo ei, CancellationToken cancellationToken)
    {
        var order = _ordersReadModel.GetOrAdd(e.OrderId);
        order.Items.Add(new OrderItemDto() { OrderItemId =e.OrderItemId, ProductId = e.ProductId, });
        return Task.CompletedTask;
    }

    public Task HandleAsync(OrderItemQuantityChanged e, EventInfo ei, CancellationToken cancellationToken)
    {
        var order = _ordersReadModel.GetOrAdd(e.OrderId);
        order.Items.First(oi => oi.OrderItemId == e.OrderItemId).Quantity = e.Quantity;
        return Task.CompletedTask;
    }
}
