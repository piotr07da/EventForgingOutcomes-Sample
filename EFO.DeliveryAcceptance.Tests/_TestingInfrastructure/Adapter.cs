using EFO.DeliveryAcceptance.Application;
using EventForging;
using EventForging.DependencyInjection;
using EventOutcomes;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace EFO.DeliveryAcceptance.Tests._TestingInfrastructure;

public class Adapter : IAdapter
{
    public Adapter()
    {
        var services = new ServiceCollection();
        services.AddEventForging();
        services.AddScoped<IEventDatabase, FakeEventDatabase>();
        services.AddMediator(c =>
        {
            c.AddConsumers(typeof(HireComponentInspectorHandler).Assembly);
        });
        // register all other services here - your main application registration code and all other fakes used for your tests
        ServiceProvider = services.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; private set; }

    public Task BeforeTestAsync()
    {
        ServiceProvider = ServiceProvider.CreateScope().ServiceProvider;
        var fakeEventDatabase = ServiceProvider.GetRequiredService<IEventDatabase>() as FakeEventDatabase;
        fakeEventDatabase!.Reset();
        return Task.CompletedTask;
    }

    public Task AfterTestAsync()
    {
        return Task.CompletedTask;
    }

    public Task SetGivenEventsAsync(IDictionary<string, IEnumerable<object>> events)
    {
        var fakeEventDatabase = ServiceProvider.GetRequiredService<IEventDatabase>() as FakeEventDatabase;
        fakeEventDatabase!.StubAlreadySavedEvents(events);
        return Task.CompletedTask;
    }

    public Task<IDictionary<string, IEnumerable<object>>> GetPublishedEventsAsync()
    {
        var fakeEventDatabase = ServiceProvider.GetRequiredService<IEventDatabase>() as FakeEventDatabase;
        return Task.FromResult<IDictionary<string, IEnumerable<object>>>(fakeEventDatabase!.NewlySavedEvents);
    }

    public async Task DispatchCommandAsync(object command)
    {
        var massTransitMediator = ServiceProvider.GetRequiredService<IMediator>();
        await massTransitMediator.Publish(command);
    }
}
