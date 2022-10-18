// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentTests.cases_for_completing_component_inspection;

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
    public async Task and_given_component_measured_and_weighed_when_CompleteInspection_then_inspection_completed()
    {
        await _test
            .Given(_componentId,
                new ComponentMeasured(_componentId, 10.5, 2, 30),
                new ComponentWeighed(_componentId, 1194))
            .When(new CompleteComponentInspection(_componentId, _inspectorId))
            .Then(_componentId, new ComponentInspectionCompleted(_componentId))
            .TestAsync();
    }

    [Theory]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Special)]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Standard)]
    [InlineData(ComponentClass.Special, ComponentClass.Standard)]
    public async Task and_given_component_classified_higher_than_inspector_certification_when_CompleteInspection_then_exception_thrown_because_of_certification_requirement(ComponentClass componentClass, ComponentClass componentInspectorAllowedComponentClass)
    {
        await _test
            .Given(_componentId, new ComponentClassified(_componentId, componentClass))
            .Given(_inspectorId, new ComponentInspectorCertified(_inspectorId, componentInspectorAllowedComponentClass))
            .When(new CompleteComponentInspection(_componentId, _inspectorId))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentInspectorDoesNotHaveRequiredCertification))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_weighed_but_not_measured_when_CompleteInspection_then_exception_thrown_because_of_component_not_measured()
    {
        await _test
            .Given(_componentId, new ComponentWeighed(_componentId, 1194))
            .When(new CompleteComponentInspection(_componentId, _inspectorId))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentNotMeasured))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_measured_but_not_weighed_when_CompleteInspection_then_exception_thrown_because_of_component_not_weighed()
    {
        await _test
            .Given(_componentId, new ComponentMeasured(_componentId, 10.5, 2, 30))
            .When(new CompleteComponentInspection(_componentId, _inspectorId))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentNotWeighed))
            .TestAsync();
    }
}
