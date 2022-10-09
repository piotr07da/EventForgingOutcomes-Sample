// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentInspectorTests;

public class given_no_component_inspector_when_HireComponentInspector_then_inspector_hired_and_certified_for_Standard_class
{
    [Fact]
    public async Task check()
    {
        var inspectorId = Guid.NewGuid();

        await Test.For(inspectorId)
            .Given()
            .When(new HireComponentInspector(inspectorId))
            .ThenInOrder(
                new ComponentInspectorHired(inspectorId),
                new ComponentInspectorCertified(inspectorId, ComponentClass.Standard))
            .TestAsync();
    }
}
