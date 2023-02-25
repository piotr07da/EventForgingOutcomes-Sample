namespace EFO.Sales.Application.Commands.Orders;

public sealed record StartOrder(Guid OrderId, Guid CustomerId);
