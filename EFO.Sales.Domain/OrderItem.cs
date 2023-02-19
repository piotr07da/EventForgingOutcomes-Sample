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

    public OrderItemId Id { get; private init; }
    public ProductId ProductId { get; private init; }
    public Quantity Quantity { get; private set; }
    public Money Price { get; private set; }

    internal static OrderItem Add(Order order, OrderItemId id, Product product, Quantity quantity)
    {
        var events = order.Events;

        events.Apply(new OrderItemAdded(order.Id, id, product.Id));
        events.Apply(new OrderItemQuantityChanged(order.Id, id, quantity));

        var orderItem = order.Items.Find(id);

        RecalculatePrice(orderItem, product);

        return orderItem;
    }

    public void Remove()
    {
        Events.Apply(new OrderItemRemoved(OrderId, Id));
    }

    public void ChangeQuantity(Product product, Quantity quantity)
    {
        Events.Apply(new OrderItemQuantityChanged(OrderId, Id, quantity));

        RecalculatePrice(this, product);
    }

    private static void RecalculatePrice(OrderItem orderItem, Product product)
    {
        var quantity = orderItem.Quantity;
        var itemPrice = product.Prices.GetUnitPriceForQuantity(quantity) * quantity;
        orderItem.Events.Apply(new OrderItemPriced(orderItem.OrderId, orderItem.Id, itemPrice));
    }

    // --------------------------------------------------- APPLY EVENTS ---------------------------------------------------

    internal static void Apply(Order order, OrderItemAdded e)
    {
        var item = new OrderItem(order)
        {
            Id = OrderItemId.Restore(e.OrderItemId),
            ProductId = ProductId.Restore(e.ProductId),
        };
        order.Items.Add(item);
    }

    internal void Apply(OrderItemRemoved e)
    {
        _order.Items.Remove(this);
    }

    internal void Apply(OrderItemQuantityChanged e)
    {
        Quantity = e.Quantity;
    }

    internal void Apply(OrderItemPriced e)
    {
        Price = Money.Restore(e.Price);
    }
}
