// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.ProductProperties;
using EFO.Catalog.Domain.ProductProperties;
using EventOutcomes;
using Xunit;

namespace EFO.Catalog.Tests.tests_for_defining_properties;

public class given_no_property
{
    private readonly Test _test;
    private readonly Guid _propertyId;

    public given_no_property()
    {
        _propertyId = Guid.NewGuid();
        _test = Test.For(_propertyId);
    }

    [Fact]
    public async Task when_DefineNumericProperty_then_numeric_property_defined()
    {
        _test
            .When(new DefineNumericProperty(_propertyId, "Max Current", "A"))
            .Then(new NumericPropertyDefined(_propertyId, "Max Current", "A"));

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_DefineTextProperty_then_text_property_defined()
    {
        _test
            .When(new DefineTextProperty(_propertyId, "Kind of channel"))
            .Then(new TextPropertyDefined(_propertyId, "Kind of channel"));

        await _test.TestAsync();
    }
}
