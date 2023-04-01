namespace EFO.Sales.Application.ReadModel.Orders;

public sealed class OrderItemDto
{
    public Guid OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}