using EFO.Sales.Application.ReadModel.Orders;
using EFO.Sales.Application.ReadModel.Products;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EFO.Sales.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSalesApplicationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IOrdersReadModel, OrdersReadModel>();
        services.AddSingleton<IProductsReadModel, ProductsReadModel>();
        return services;
    }
}
