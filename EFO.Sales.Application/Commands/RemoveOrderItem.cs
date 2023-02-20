namespace EFO.Sales.Application.Commands;

public sealed record RemoveOrderItem(Guid OrderId, Guid OrderItemId);
