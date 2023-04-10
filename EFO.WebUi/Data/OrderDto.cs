namespace EFO.WebUi.Data;

public sealed record OrderDto(Guid OrderId, Guid CustomerId, OrderDto.Item[] Items)
{
    public sealed record Item(Guid OrderItemId, Guid ProductId, int Quantity);
}
