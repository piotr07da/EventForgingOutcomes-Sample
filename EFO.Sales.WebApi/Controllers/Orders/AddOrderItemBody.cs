namespace EFO.Sales.WebApi.Controllers.Orders;

public sealed record AddOrderItemBody(Guid ProductId, int Quantity);
