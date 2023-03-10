using EFO.SharedReadModel.ReadModel.Products;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EFO.SharedReadModel;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedReadModel(this IServiceCollection services)
    {
        services.AddSingleton<IProductsReadModel, ProductsReadModel>();
        services.AddSingleton<ICategoriesReadModel, CategoriesReadModel>();
        services.AddSingleton<IPropertiesReadModel, PropertiesReadModel>();
        return services;
    }
}
