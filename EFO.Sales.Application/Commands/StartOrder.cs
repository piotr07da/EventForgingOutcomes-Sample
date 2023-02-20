namespace EFO.Sales.Application.Commands;

public sealed record StartOrder(Guid OrderId, Guid CustomerId);
