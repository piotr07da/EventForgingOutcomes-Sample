// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentInspectorTests;

public class public_properties_checks
{
    private readonly Guid _componentInspectorId;
    private readonly Test _test;

    public public_properties_checks()
    {
        _componentInspectorId = Guid.NewGuid();
        _test = Test.For(_componentInspectorId);
    }

    [Fact]
    public async Task given_component_inspector_hired_then_Id_set()
    {
        await _test
            .Given(new ComponentInspectorHired(_componentInspectorId))
            .ThenAggregate<ComponentInspector>(inspector => inspector.Id.Value == _componentInspectorId)
            .TestAsync();
    }

    [Theory]
    [InlineData(ComponentClass.Special)]
    [InlineData(ComponentClass.Hazardous)]
    public async Task given_component_inspector_certified_then_AllowedComponentClass_set(ComponentClass componentClass)
    {
        await _test
            .Given(
                new ComponentInspectorHired(_componentInspectorId),
                new ComponentInspectorCertified(_componentInspectorId, componentClass))
            .ThenAggregate<ComponentInspector>(inspector => inspector.AllowedComponentClass == componentClass)
            .TestAsync();
    }
}
