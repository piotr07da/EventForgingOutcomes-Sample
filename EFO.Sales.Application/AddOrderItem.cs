namespace EFO.Sales.Application;

public sealed record AddOrderItem(Guid OrderId, Guid OrderItemId, Guid ProductId, int Quantity);
