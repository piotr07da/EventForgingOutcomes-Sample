// ReSharper disable InconsistentNaming

using EFO.DeliveryAcceptance.Application;
using EFO.DeliveryAcceptance.Domain;
using EFO.DeliveryAcceptance.Tests._TestingInfrastructure;
using EventOutcomes;
using Xunit;

namespace EFO.DeliveryAcceptance.Tests.ComponentTests.cases_for_measuring_component;

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
    public async Task when_MeasureComponent_then_component_measured()
    {
        await _test
            .When(new MeasureComponent(_componentId, _inspectorId, 10, 20, 30.5))
            .Then(_componentId, new ComponentMeasured(_componentId, 10, 20, 30.5))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_already_measured_when_MeasureComponent_then_component_measured_again()
    {
        await _test
            .Given(_componentId, new ComponentMeasured(_componentId, 5, 5, 5))
            .When(new MeasureComponent(_componentId, _inspectorId, 10, 20, 30.5))
            .Then(_componentId, new ComponentMeasured(_componentId, 10, 20, 30.5))
            .TestAsync();
    }

    [Fact]
    public async Task and_given_component_inspection_already_completed_when_MeasureComponent_then_exception_thrown()
    {
        await _test
            .Given(_componentId, new ComponentInspectionCompleted(_componentId))
            .When(new MeasureComponent(_componentId, _inspectorId, 10, 10, 10))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentInspectionAlreadyCompleted))
            .TestAsync();
    }

    [Theory]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Special)]
    [InlineData(ComponentClass.Hazardous, ComponentClass.Standard)]
    [InlineData(ComponentClass.Special, ComponentClass.Standard)]
    public async Task and_given_component_classified_higher_than_inspector_certification_when_MeasureComponent_then_exception_thrown_because_of_certification_requirement(ComponentClass componentClass, ComponentClass componentInspectorAllowedComponentClass)
    {
        await _test
            .Given(_componentId, new ComponentClassified(_componentId, componentClass))
            .Given(_inspectorId, new ComponentInspectorCertified(_inspectorId, componentInspectorAllowedComponentClass))
            .When(new MeasureComponent(_componentId, _inspectorId, 10, 20, 30.5))
            .ThenException<DomainException>(de => de.Errors.Contains(DomainErrors.ComponentInspectorDoesNotHaveRequiredCertification))
            .TestAsync();
    }
}
