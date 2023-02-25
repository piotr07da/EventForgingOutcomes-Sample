namespace EFO.Sales.Application.Queries.Orders;

public record GetOrder
{
    public Guid OrderId { get; init; }
}
