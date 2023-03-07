using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace EFO.Catalog.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogApplicationLayer(this IServiceCollection services)
    {
        return services;
    }
}
