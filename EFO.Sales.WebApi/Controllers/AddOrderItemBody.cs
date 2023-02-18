namespace EFO.Sales.WebApi.Controllers;

public sealed record AddOrderItemBody(Guid ProductId, int Quantity);
