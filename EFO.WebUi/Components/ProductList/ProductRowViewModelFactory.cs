using EFO.WebUi.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace EFO.WebUi.Components.ProductList;

public class ProductRowViewModelFactory : IProductRowViewModelFactory
{
    private readonly IOrderService _orderService;
    private readonly ProtectedLocalStorage _localStorage;

    public ProductRowViewModelFactory(IOrderService orderService, ProtectedLocalStorage localStorage)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public ProductRowViewModel Create(ProductDto product)
    {
        return new ProductRowViewModel(product, _orderService, _localStorage);
    }
}
