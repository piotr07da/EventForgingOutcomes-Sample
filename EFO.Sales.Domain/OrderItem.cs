using EventForging;

namespace EFO.Sales.Domain;

public sealed class OrderItem
{
    private readonly Order _order;

    private OrderItem(Order order)
    {
        _order = order;
    }

    private Events Events => _order.Events;
    private OrderId OrderId => _order.Id;

    public OrderItemId Id { get; private set; }
    public Money Price { get; private set; }

    internal static OrderItem Add(Order order, OrderItemId id, Product product, Quantity quantity)
    {
        var events = order.Events;

        events.Apply(new OrderItemAdded(order.Id, id, product.Id));
        events.Apply(new OrderItemQuantityChanged(order.Id, id, quantity));

        var itemPrice = product.Prices.GetUnitPriceForQuantity(quantity) * quantity;
        events.Apply(new OrderItemPriced(order.Id, id, itemPrice));

        return order.Items.Find(id);
    }

    public void Remove()
    {
        Events.Apply(new OrderItemRemoved(OrderId, Id));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    internal static void Apply(Order order, OrderItemAdded e)
    {
        var item = new OrderItem(order)
        {
            Id = OrderItemId.Restore(e.OrderItemId),
        };
        order.Items.Add(item);
    }

    internal void Apply(OrderItemRemoved e)
    {
        _order.Items.Remove(this);
    }

    internal void Apply(OrderItemQuantityChanged e)
    {
    }

    internal void Apply(OrderItemPriced e)
    {
        Price = Money.Restore(e.Price);
    }
}
