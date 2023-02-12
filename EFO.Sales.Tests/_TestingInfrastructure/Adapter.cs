using EFO.Sales.Application;
using EventForging;
using EventForging.DependencyInjection;
using EventOutcomes;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace EFO.Sales.Tests._TestingInfrastructure;

public class Adapter : IAdapter
{
    public Adapter()
    {
        var services = new ServiceCollection();
        services.AddEventForging(c => { });
        services.AddSingleton<IEventDatabase, FakeEventDatabase>();
        services.AddMediator(c =>
        {
            var asm = typeof(StartOrderConsumer).Assembly;
            c.AddConsumers(asm);
        });
        // register all other services here - your main application registration code and all other fakes used for your tests
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
