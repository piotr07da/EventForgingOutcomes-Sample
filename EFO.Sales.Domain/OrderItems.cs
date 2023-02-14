using System.Collections;

namespace EFO.Sales.Domain;

public sealed class OrderItems : IReadOnlyList<OrderItem>
{
    private readonly IList<OrderItem> _items;

    public OrderItems()
    {
        _items = new List<OrderItem>();
    }

    public int Count => _items.Count;

    public OrderItem this[int index] => _items[index];

    public IEnumerator<OrderItem> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Contains(OrderItemId itemId)
    {
        return _items.Any(i => i.Id == itemId);
    }

    public OrderItem Find(OrderItemId itemId)
    {
        foreach (var item in _items)
        {
            if (item.Id == itemId)
            {
                return item;
            }
        }

        throw new DomainException(new DomainError(DomainErrors.OrderItemWithGivenIdNotFound).WithData("ItemId", itemId));
    }

    public void Add(OrderItem item)
    {
        _items.Add(item);
    }

    public void Remove(OrderItem item)
    {
        _items.Remove(item);
    }
}
