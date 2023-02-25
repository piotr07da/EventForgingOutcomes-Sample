// ReSharper disable InconsistentNaming

using EFO.Shared.Domain;
using Xunit;

namespace EFO.Sales.Tests;

public sealed class value_objects_tests_for_ProductName
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ProductName_cannot_be_empty(string emptyProductName)
    {
        var ex = Assert.Throws<DomainException>(() => ProductName.FromValue(emptyProductName));
        Assert.Contains(ex.Errors, e => e.Name == DomainErrors.ProductNameCannotBeEmpty);
    }
}
