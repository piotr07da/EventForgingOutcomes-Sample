using EventForging;

namespace EFO.Sales.Domain;

public class Order : IEventForged
{
    public Order()
    {
        Events = Events.CreateFor(this);
        Errors = new List<string>();
    }

    public Events Events { get; }
    public IList<string> Errors { get; }

    public OrderId Id { get; private set; }

    public static Order Start(OrderId orderId, CustomerId customerId)
    {
        var order = new Order();
        var events = order.Events;
        events.Apply(new OrderStarted(orderId));
        events.Apply(new OrderCustomerAssigned(orderId, customerId));
        return order;
    }

    public void AddItem(OrderItemId itemId, Product product, Quantity quantity)
    {
        Events.Apply(new OrderItemAdded(Id, itemId, product.Id));
        Events.Apply(new OrderItemQuantityChanged(Id, itemId, quantity));

        var itemPrice = product.Prices.GetUnitPriceForQuantity(quantity) * quantity;
        Events.Apply(new OrderItemPriced(Id, itemId, itemPrice));
        Events.Apply(new OrderPriced(Id, itemPrice));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(OrderStarted e)
    {
        Id = OrderId.Restore(e.OrderId);
    }

    private void Apply(OrderCustomerAssigned e)
    {
    }

    private void Apply(OrderItemAdded e)
    {
    }

    private void Apply(OrderItemQuantityChanged e)
    {
    }

    private void Apply(OrderItemPriced e)
    {
    }

    private void Apply(OrderPriced e)
    {
    }
}
