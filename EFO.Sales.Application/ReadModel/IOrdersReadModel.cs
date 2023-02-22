namespace EFO.Sales.Application.ReadModel;

public interface IOrdersReadModel
{
    OrderDto Get(Guid orderId);

    OrderDto GetOrAdd(Guid orderId);
}
