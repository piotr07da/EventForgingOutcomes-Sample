﻿// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.Products;
using EFO.Catalog.Domain.ProductProperties;
using EFO.Catalog.Domain.Products;
using EventForging;
using EventOutcomes;
using Xunit;

namespace EFO.Catalog.Tests.tests_for_setting_product_properties;

public sealed class given_product_and_numeric_property
{
    private readonly Test _test;

    private readonly Guid _productId;
    private readonly Guid _propertyId;

    public given_product_and_numeric_property()

    {
        _productId = Guid.NewGuid();
        _propertyId = Guid.NewGuid();

        _test = Test.ForMany()
            .Given(_productId, new ProductIntroduced(_productId))
            .Given(_propertyId, new NumericPropertyDefined(_propertyId, "Max Current", "V"));
    }

    [Fact]
    public async Task when_SetProductNumericProperty_then_numeric_property_set()
    {
        _test
            .When(new SetProductNumericProperty(_productId, _propertyId, 3.31m))
            .Then(_productId, new ProductNumericPropertySet(_productId, _propertyId, 3.31m));

        await _test.TestAsync();
    }

    [Fact]
    public async Task when_SetProductNumericProperty_for_not_existing_property_then_exception_thrown()
    {
        var notExistingPropertyId = Guid.NewGuid();

        _test
            .When(new SetProductNumericProperty(_productId, notExistingPropertyId, 3.31m))
            .ThenException<AggregateNotFoundEventForgingException>();

        await _test.TestAsync();
    }
}
