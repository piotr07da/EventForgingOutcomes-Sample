using EFO.WebUi.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ReactiveUI;
using System.Reactive;

namespace EFO.WebUi.Components.ProductList;

public class ProductRowViewModel : ReactiveObject
{
    private int _quantity;

    public ProductRowViewModel(ProductDto product, IOrderService orderService, ProtectedLocalStorage localStorage)
    {
        Product = product;

        AddToOrderCommand = ReactiveCommand.CreateFromTask(async () => {
            var orderId = await localStorage.GetAsync<Guid>("orderId");
            await orderService.AddOrderItemAsync(orderId.Value, new AddOrderItemDto(product.ProductId, Quantity));
        });
    }

    public ProductDto Product { get; }

    public int Quantity
    {
        get => _quantity;
        set => this.RaiseAndSetIfChanged(ref _quantity, value);
    }

    public ReactiveCommand<Unit, Unit> AddToOrderCommand { get; }
}