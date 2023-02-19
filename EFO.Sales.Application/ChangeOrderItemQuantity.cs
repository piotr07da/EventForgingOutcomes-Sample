namespace EFO.Sales.Application;

public sealed record ChangeOrderItemQuantity(Guid OrderId, Guid OrderItemId, int Quantity);
