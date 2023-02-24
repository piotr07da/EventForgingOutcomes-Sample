namespace EFO.Sales.Application.ReadModel.Orders;

public interface IOrdersReadModel
{
    OrderDto Get(Guid orderId);

    OrderDto GetOrAdd(Guid orderId);
}
