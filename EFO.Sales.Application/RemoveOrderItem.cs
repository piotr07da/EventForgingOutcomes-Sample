namespace EFO.Sales.Application;

public sealed record RemoveOrderItem(Guid OrderId, Guid OrderItemId);
