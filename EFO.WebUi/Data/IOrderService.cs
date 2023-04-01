using Refit;

namespace EFO.WebUi.Data;

public interface IOrderService
{
    [Get("/orders/{orderId}")]
    Task<OrderDto> GetOrderAsync(Guid orderId);

    [Post("/orders")]
    Task<Guid> StartOrderAsync(StartOrderDto model);

    [Post("/orders/{orderId}/items")]
    Task AddOrderItemAsync(Guid orderId, AddOrderItemDto model);
}
