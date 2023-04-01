namespace EFO.Sales.Application.ReadModel.Orders;

public sealed class OrderDto
{
    public OrderDto(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public IList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}
