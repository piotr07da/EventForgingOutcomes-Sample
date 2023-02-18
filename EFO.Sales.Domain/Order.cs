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
    public OrderItems Items { get; } = new();

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
        OrderItem.Add(this, itemId, product, quantity);

        var orderPrice = Items.SumUpPrices();

        Events.Apply(new OrderPriced(Id, orderPrice));
    }

    public void RemoveItem(OrderItemId itemId)
    {
        Item(itemId).Remove();

        var orderPrice = Items.SumUpPrices();

        Events.Apply(new OrderPriced(Id, orderPrice));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    private void Apply(OrderStarted e)
    {
        Id = OrderId.Restore(e.OrderId);
    }

    private void Apply(OrderCustomerAssigned e)
    {
    }

    private void Apply(OrderPriced e)
    {
    }

    private void Apply(OrderItemAdded e) => OrderItem.Apply(this, e);

    private void Apply(OrderItemRemoved e) => Item(e.OrderItemId).Apply(e);

    private void Apply(OrderItemQuantityChanged e) => Item(e.OrderItemId).Apply(e);

    private void Apply(OrderItemPriced e) => Item(e.OrderItemId).Apply(e);

    private OrderItem Item(OrderItemId itemId) => Items.Find(itemId);
}
