// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.Categories;
using EFO.Catalog.Domain.Categories;
using EventOutcomes;
using Xunit;

namespace EFO.Catalog.Tests.tests_for_adding_categories;

public class given_no_category
{
    private readonly Test _test;
    private readonly Guid _categoryId;

    public given_no_category()
    {
        _categoryId = Guid.NewGuid();

        _test = Test.For(_categoryId);
    }

    [Fact]
    public async Task when_AddCategory_then_category_added()
    {
        _test
            .When(new AddCategory(_categoryId, "Semiconductors"))
            .ThenInOrder(
                new CategoryAdded(_categoryId),
                new CategoryNamed(_categoryId, "Semiconductors"));

        await _test.TestAsync();
    }
}
