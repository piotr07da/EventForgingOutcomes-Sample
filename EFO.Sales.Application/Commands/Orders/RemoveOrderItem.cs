namespace EFO.Sales.Application.Commands.Orders;

public sealed record RemoveOrderItem(Guid OrderId, Guid OrderItemId);
