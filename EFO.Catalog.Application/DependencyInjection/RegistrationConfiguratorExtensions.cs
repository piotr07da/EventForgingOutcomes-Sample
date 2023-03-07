using EFO.Catalog.Application.Commands.Products;
using MassTransit;

// ReSharper disable once CheckNamespace
namespace EFO.Sales.Application;

public static class RegistrationConfiguratorExtensions
{
    public static void AddCatalogApplicationLayerConsumers(this IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumers(typeof(IntroduceProductHandler).Assembly);
    }
}
