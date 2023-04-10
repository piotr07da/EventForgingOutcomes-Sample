using EFO.WebUi.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ReactiveUI;

namespace EFO.WebUi.Pages;

public class CartViewModel : ReactiveObject
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly IOrderService _orderService;

    private OrderDto? _order;

    public CartViewModel(ProtectedLocalStorage localStorage, IOrderService orderService)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public OrderDto? Order
    {
        get => _order;
        set => this.RaiseAndSetIfChanged(ref _order, value);
    }

    public async Task LoadOrderAsync()
    {
        var orderIdResult = await _localStorage.GetAsync<Guid>("orderId");
        if (orderIdResult.Success)
        {
            Order = await _orderService.GetOrderAsync(orderIdResult.Value);
        }
    }
}
