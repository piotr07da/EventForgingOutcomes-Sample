using EFO.Catalog.Application;
using EFO.Catalog.Application.Commands.Products;
using EFO.Shared.Domain;
using EFO.Shared.Tests._TestingInfrastructure;
using EventForging;
using EventOutcomes;

namespace EFO.Catalog.Tests;

internal static class TestExtensions
{
    public static async Task TestAsync(this Test test)
    {
        await Tester.TestAsync(test, new Adapter(typeof(IntroduceProductHandler).Assembly, services =>
        {
            services.AddCatalogApplicationLayer();
        }));
    }

    public static Test ThenAggregate<TAggregate>(this Test test, Guid aggregateId, Func<TAggregate, bool> assertion)
    {
        return test.Then<IRepository<TAggregate>>(async r => assertion(await r.GetAsync(aggregateId)));
    }

    public static Test ThenDomainExceptionWith(this Test test, string domainError)
    {
        return test.ThenAnyException<DomainException>(dex => dex.Errors.Any(der => der.Name == domainError));
    }
}
