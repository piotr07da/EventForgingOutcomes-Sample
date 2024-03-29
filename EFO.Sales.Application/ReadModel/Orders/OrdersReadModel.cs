﻿namespace EFO.Sales.Application.ReadModel.Orders;

internal sealed class OrdersReadModel : IOrdersReadModel
{
    private readonly IDictionary<Guid, OrderDto> _entries = new Dictionary<Guid, OrderDto>();

    public OrderDto[] GetAll()
    {
        return _entries.Values.ToArray();
    }

    public OrderDto Get(Guid orderId)
    {
        return _entries[orderId];
    }

    public OrderDto GetOrAdd(Guid orderId)
    {
        if (!_entries.TryGetValue(orderId, out var order))
        {
            order = new OrderDto(orderId);
            _entries.Add(orderId, order);
        }

        return order;
    }
}
