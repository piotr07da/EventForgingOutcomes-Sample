// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.Categories;
using EFO.Catalog.Domain.Categories;
using EventOutcomes;
using Xunit;

namespace EFO.Catalog.Tests.tests_for_adding_categories;

public class given_category
{
    private readonly Test _test;
    private readonly Guid _categoryId;
    private readonly Guid _subcategoryId;

    public given_category()
    {
        _categoryId = Guid.NewGuid();
        _subcategoryId = Guid.NewGuid();

        _test = Test.ForMany()
            .Given(_categoryId, new CategoryAdded(_categoryId));
    }

    [Fact]
    public async Task when_AddSubcategory_then_subcategory_added()
    {
        _test
            .When(new AddSubcategory(_categoryId, _subcategoryId, "Diodes"))
            .Then(
                _subcategoryId,
                new CategoryAdded(_subcategoryId))
            .ThenInAnyOrder(
                _subcategoryId,
                new CategoryNamed(_subcategoryId, "Diodes"),
                new CategoryAttachedToParent(_subcategoryId, _categoryId));

        await _test.TestAsync();
    }
}
