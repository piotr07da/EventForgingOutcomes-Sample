namespace EFO.Sales.Application.ReadModel;

public class OrderDto
{
    public OrderDto(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
}
