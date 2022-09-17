// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentInspectorTests;

public class given_no_component_inspector_when_HireComponentInspector_then_inspector_hired_and_certified
{
    [Theory]
    [InlineData(ComponentInspectorCertificationLevel.Advanced)]
    [InlineData(ComponentInspectorCertificationLevel.Master)]
    public async Task check(ComponentInspectorCertificationLevel certificationLevel)
    {
        var inspectorId = Guid.NewGuid();

        await Test.For(inspectorId)
            .Given()
            .When(new HireComponentInspector(inspectorId, certificationLevel))
            .ThenInOrder(
                new ComponentInspectorHired(inspectorId),
                new ComponentInspectorCertified(certificationLevel))
            .TestAsync();
    }
}
