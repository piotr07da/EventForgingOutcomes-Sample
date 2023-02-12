using EventForging;

namespace EFO.Sales.Domain;

public class Order : IEventForged
{
    public Order()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public OrderId Id { get; private set; }

    public static Order Start(OrderId orderId, CustomerId customerId)
    {
        var order = new Order();
        var events = order.Events;
        events.Apply(new OrderStarted(orderId));
        events.Apply(new OrderCustomerAssigned(orderId, customerId));
        return order;
    }

    public void AddItem(OrderItemId itemId, ProductId productId, Quantity quantity)
    {
        Events.Apply(new OrderItemAdded(Id, itemId, productId, quantity));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(OrderStarted e)
    {
    }

    private void Apply(OrderCustomerAssigned e)
    {
    }

    private void Apply(OrderItemAdded e)
    {
    }
}
