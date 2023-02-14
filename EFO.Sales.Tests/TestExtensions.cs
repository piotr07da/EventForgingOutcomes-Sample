using EFO.Sales.Domain;
using EventOutcomes;

namespace EFO.Sales.Tests;

internal static class TestExtensions
{
    public static Test ThenDomainExceptionWith(this Test test, string domainError)
    {
        return test.ThenAnyException<DomainException>(de => de.Errors.Any(de => de.Name == domainError));
    }
}
