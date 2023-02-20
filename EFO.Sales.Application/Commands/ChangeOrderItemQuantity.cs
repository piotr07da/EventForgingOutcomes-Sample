namespace EFO.Sales.Application.Commands;

public sealed record ChangeOrderItemQuantity(Guid OrderId, Guid OrderItemId, int Quantity);
