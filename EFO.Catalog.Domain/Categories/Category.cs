using EventForging;

namespace EFO.Catalog.Domain.Categories;

public class Category : IEventForged
{
    public Category()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public CategoryId Id { get; private set; }

    public static Category Add(CategoryId id, CategoryName name)
    {
        var category = new Category();

        var events = category.Events;

        events.Apply(new CategoryAdded(id));
        events.Apply(new CategoryNamed(id, name));

        return category;
    }

    public Category AddSubcategory(CategoryId subcategoryId, CategoryName subcategoryName)
    {
        var subcategory = Add(subcategoryId, subcategoryName);
        subcategory.Events.Apply(new CategoryAttachedToParent(subcategoryId, Id));
        return subcategory;
    }

    private void Apply(CategoryAdded e)
    {
        Id = CategoryId.Restore(e.CategoryId);
    }

    private void Apply(CategoryNamed e)
    {
    }

    private void Apply(CategoryAttachedToParent e)
    {
    }
}
