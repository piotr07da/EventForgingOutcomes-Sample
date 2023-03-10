using EFO.SharedReadModel.Queries;
using MassTransit;

// ReSharper disable once CheckNamespace
namespace EFO.SharedReadModel;

public static class RegistrationConfiguratorExtensions
{
    public static void AddSharedReadModelConsumers(this IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumers(typeof(GetProducts).Assembly);
    }
}
