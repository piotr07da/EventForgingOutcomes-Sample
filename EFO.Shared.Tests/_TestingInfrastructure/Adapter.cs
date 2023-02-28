using System.Reflection;
using EFO.Shared.Application.MassTransit;
using EventForging;
using EventOutcomes;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace EFO.Shared.Tests._TestingInfrastructure;

public class Adapter : IAdapter
{
    public Adapter(Assembly applicationLayerAssembly, Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.AddEventForging(c => { });
        services.AddSingleton<IEventDatabase, FakeEventDatabase>();
        services.AddMediator(c =>
        {
            c.AddConsumers(applicationLayerAssembly);

            c.ConfigureMediator((registrationContext, configurator) =>
            {
                configurator.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(registrationContext));
            });
        });

        services.AddLogging();
        services.AddLocalization(lo => lo.ResourcesPath = "");

        configure(services);

        ServiceProvider = services.BuildServiceProvider();

        FakeEventDatabase.Initialize();
    }

    public IServiceProvider ServiceProvider { get; private set; }

    private FakeEventDatabase FakeEventDatabase => (ServiceProvider.GetRequiredService<IEventDatabase>() as FakeEventDatabase)!;
    private IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();

    public Task BeforeTestAsync()
    {
        ServiceProvider = ServiceProvider.CreateScope().ServiceProvider;
        return Task.CompletedTask;
    }

    public Task AfterTestAsync()
    {
        return Task.CompletedTask;
    }

    public Task SetGivenEventsAsync(IDictionary<string, IEnumerable<object>> events)
    {
        FakeEventDatabase.StubAlreadySavedEvents(events);
        return Task.CompletedTask;
    }

    public Task<IDictionary<string, IEnumerable<object>>> GetPublishedEventsAsync()
    {
        return Task.FromResult<IDictionary<string, IEnumerable<object>>>(FakeEventDatabase.NewlySavedEvents);
    }

    public async Task DispatchCommandAsync(object command)
    {
        await Mediator.Publish(command);
    }
}
