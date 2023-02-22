using EFO.Sales.Application.Commands;
using MassTransit;

// ReSharper disable once CheckNamespace
namespace EFO.Sales.Application;

public static class RegistrationConfiguratorExtensions
{
    public static void AddSalesApplicationLayerConsumers(this IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumers(typeof(StartOrderHandler).Assembly);
    }
}
