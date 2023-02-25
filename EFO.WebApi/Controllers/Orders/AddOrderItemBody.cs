namespace EFO.WebApi.Controllers.Orders;

public sealed record AddOrderItemBody(Guid ProductId, int Quantity);
