// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentTests.cases_for_registering_arrival;

public class given_no_Component_when_RegisterArrival_with_incorrect_name_then_exception_thrown
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task verify(string incorrectName)
    {
        var componentId = Guid.NewGuid();

        await Test.For(componentId)
            .Given()
            .When(new RegisterComponentArrival(componentId, incorrectName))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentNameIsInvalid))
            .TestAsync();
    }
}
