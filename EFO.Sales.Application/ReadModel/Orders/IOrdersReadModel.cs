namespace EFO.Sales.Application.ReadModel.Orders;

public interface IOrdersReadModel
{
    OrderDto[] GetAll();

    OrderDto Get(Guid orderId);

    OrderDto GetOrAdd(Guid orderId);
}
