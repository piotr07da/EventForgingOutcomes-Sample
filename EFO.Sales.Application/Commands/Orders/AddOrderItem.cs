namespace EFO.Sales.Application.Commands.Orders;

public sealed record AddOrderItem(Guid OrderId, Guid OrderItemId, Guid ProductId, int Quantity);
