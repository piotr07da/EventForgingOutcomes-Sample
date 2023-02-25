// ReSharper disable InconsistentNaming

using EFO.Shared.Domain;
using Xunit;

namespace EFO.Sales.Tests;

public sealed class value_objects_tests_for_ProductId
{
    [Fact]
    public void ProductId_cannot_be_empty()
    {
        var ex = Assert.Throws<DomainException>(() => ProductId.FromValue(Guid.Empty));
        Assert.Contains(ex.Errors, e => e.Name == DomainErrors.ProductIdCannotBeEmpty);
    }
}
