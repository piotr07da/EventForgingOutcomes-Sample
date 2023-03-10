using EFO.Sales.Application;
using EFO.Shared.Application.MassTransit;
using EFO.SharedReadModel;
using MassTransit;

namespace EFO.WebApi.ServiceCollectionExtensions;

internal static class MassTransitServiceCollectionExtensions
{
    public static void AddAndConfigureMediator(this IServiceCollection services)
    {
        services.AddMediator(r =>
        {
            r.AddCatalogApplicationLayerConsumers();
            r.AddSalesApplicationLayerConsumers();
            r.AddSharedReadModelConsumers();

            r.ConfigureMediator((rc, c) =>
            {
                c.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(rc));
            });
        });
    }

    public static void AddAndConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(r =>
        {
            r.AddCatalogApplicationLayerConsumers();
            r.AddSalesApplicationLayerConsumers();
            r.AddSharedReadModelConsumers();

            r.UsingInMemory((rc, c) =>
            {
                c.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(rc));
            });
        });
    }
}
