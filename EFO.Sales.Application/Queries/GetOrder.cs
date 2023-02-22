namespace EFO.Sales.Application.Queries;

public record GetOrder
{
    public Guid OrderId { get; init; }
}
