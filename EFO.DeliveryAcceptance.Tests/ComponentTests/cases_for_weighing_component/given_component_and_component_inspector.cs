// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentTests.cases_for_weighing_component;

public class given_component_and_component_inspector
{
    private readonly Guid _componentId;
    private readonly Guid _inspectorId;
    private readonly Test _test;

    public given_component_and_component_inspector()
    {
        _componentId = Guid.NewGuid();
        _inspectorId = Guid.NewGuid();
        _test = Test.ForMany()
            .Given(_componentId,
                new ComponentArrived(_componentId),
                new ComponentClassified(_componentId, ComponentClass.Standard))
            .Given(_inspectorId,
                new ComponentInspectorHired(_inspectorId),
                new ComponentInspectorCertified(_inspectorId, ComponentClass.Standard));
    }

    [Fact]
    public async Task when_WeighComponent_then_component_weighed()
    {
        await _test
            .When(new WeighComponent(_componentId, _inspectorId, 100))
            .Then(_componentId, new ComponentWeighed(_componentId, 100))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_already_weighed_when_WeighComponent_then_component_weighed_again()
    {
        await _test
            .Given(_componentId, new ComponentWeighed(_componentId, 50))
            .When(new WeighComponent(_componentId, _inspectorId, 55.55))
            .Then(_componentId, new ComponentWeighed(_componentId, 55.55))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_inspection_already_completed_when_WeighComponent_then_exception_thrown()
    {
        await _test
            .Given(_componentId, new ComponentInspectionCompleted(_componentId))
            .When(new WeighComponent(_componentId, _inspectorId, 200))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentInspectionAlreadyCompleted))
            .TestAsync();
    }

    [Theory]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Special)]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Standard)]
    [InlineData(ComponentClass.Special, ComponentClass.Standard)]
    public async Task and_given_component_classified_higher_than_inspector_certification_when_WeighComponent_then_exception_thrown_because_of_certification_requirement(ComponentClass componentClass, ComponentClass componentInspectorAllowedComponentClass)
    {
        await _test
            .Given(_componentId, new ComponentClassified(_componentId, componentClass))
            .Given(_inspectorId, new ComponentInspectorCertified(_inspectorId, componentInspectorAllowedComponentClass))
            .When(new WeighComponent(_componentId, _inspectorId, 120))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentInspectorDoesNotHaveRequiredCertification))
            .TestAsync();
    }
}
