using EFO.Sales.Application;
using EFO.Sales.Application.Commands.Orders;
using EFO.Shared.Tests._TestingInfrastructure;
using EventForging;
using EventOutcomes;

namespace EFO.Sales.Tests._TestingInfrastructure;

public static class TestExtensions
{
    public static async Task TestAsync(this Test test)
    {
        await Tester.TestAsync(test, new Adapter(typeof(StartOrderHandler).Assembly, services =>
        {
            services.AddSalesApplicationLayer();
        }));
    }

    public static Test ThenAggregate<TAggregate>(this Test test, Guid aggregateId, Func<TAggregate, bool> assertion)
    {
        return test.Then<IRepository<TAggregate>>(async r => assertion(await r.GetAsync(aggregateId)));
    }
}
