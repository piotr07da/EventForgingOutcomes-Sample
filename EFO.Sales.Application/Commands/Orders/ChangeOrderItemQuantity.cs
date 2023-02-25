namespace EFO.Sales.Application.Commands.Orders;

public sealed record ChangeOrderItemQuantity(Guid OrderId, Guid OrderItemId, int Quantity);
