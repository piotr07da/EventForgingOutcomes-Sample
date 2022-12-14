using EventForging;
using EventOutcomes;

namespace EFO.DeliveryAcceptance.Tests._TestingInfrastructure;

public static class TestExtensions
{
    public static async Task TestAsync(this Test test)
    {
        await Tester.TestAsync(test, new Adapter());
    }

    public static Test ThenAggregate<TAggregate>(this Test test, Func<TAggregate, bool> assertion)
    {
        return test.Then<IRepository<TAggregate>>(async r => assertion(await r.GetAsync(test.EventStreamId)));
    }

    public static Test ThenAggregate<TAggregate>(this Test test, Guid aggregateId, Func<TAggregate, bool> assertion)
    {
        return test.Then<IRepository<TAggregate>>(async r => assertion(await r.GetAsync(aggregateId)));
    }
}
