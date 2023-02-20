namespace EFO.Sales.Application.Commands;

public sealed record AddOrderItem(Guid OrderId, Guid OrderItemId, Guid ProductId, int Quantity);
