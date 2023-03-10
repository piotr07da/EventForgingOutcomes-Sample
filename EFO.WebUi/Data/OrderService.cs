using System.Text.Json;

namespace EFO.WebUi.Data;

public sealed class OrderService : IOrderService
{
    private readonly string _efoAddress;

    private Guid? _orderId;


    public OrderService(IConfiguration configuration)
    {
        _efoAddress = configuration["Services:EFO"]!;
    }

    public async Task AddOrderItemAsync(AddOrderItemDto item)
    {
        var orderId = await EnsureOrderExist();
        using var httpClient = new HttpClient();
        var uri = new Uri($"{_efoAddress}/orders/{orderId}/items");
        await httpClient.PostAsync(uri, JsonContent.Create(item));
    }

    private async Task<Guid> EnsureOrderExist()
    {
        if (_orderId is null)
        {
            var httpClient = new HttpClient();
            var r = await httpClient.PostAsync(new Uri($"{_efoAddress}/orders"), JsonContent.Create(new { CustomerId = Guid.NewGuid(), }));
            var id = await r.Content.ReadAsStringAsync();
            _orderId = JsonSerializer.Deserialize<Guid>(id);
        }

        return _orderId.Value;
    }
}
