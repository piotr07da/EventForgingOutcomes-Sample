using System.Text.Json;

namespace EFO.WebUi.Data;

public sealed class OrderService : IOrderService
{
    private Guid? _orderId;

    public async Task AddOrderItemAsync(AddOrderItemDto item)
    {
        var orderId = await EnsureOrderExist();
        using var httpClient = new HttpClient();
        var uri = new Uri($"http://localhost:5182/orders/{orderId}/items");
        await httpClient.PostAsync(uri, JsonContent.Create(item));
    }

    private async Task<Guid> EnsureOrderExist()
    {
        if (_orderId is null)
        {
            var httpClient = new HttpClient();
            var r = await httpClient.PostAsync(new Uri("http://localhost:5182/orders"), JsonContent.Create(new { CustomerId = Guid.NewGuid(), }));
            var id = await r.Content.ReadAsStringAsync();
            _orderId = JsonSerializer.Deserialize<Guid>(id);
        }

        return _orderId.Value;
    }
}
