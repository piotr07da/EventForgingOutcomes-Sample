// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentTests.cases_for_registering_arrival;

public class given_no_Component_when_RegisterArrival_then_component_arrived_and_named
{
    [Fact]
    public async Task verify()
    {
        var componentId = Guid.NewGuid();
        var componentName = "Some new component name";

        await Test.For(componentId)
            .Given()
            .When(new RegisterComponentArrival(componentId, componentName))
            .ThenInOrder(
                new ComponentArrived(componentId),
                new ComponentNamed(componentId, componentName))
            .TestAsync();
    }
}
