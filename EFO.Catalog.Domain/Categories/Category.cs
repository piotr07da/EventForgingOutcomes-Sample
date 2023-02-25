using EventForging;

namespace EFO.Catalog.Domain.Categories;

public class Category : IEventForged
{
    private Category()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public static Category Create(CategoryId id, CategoryName name)
    {
        var category = new Category();

        return category;
    }
}
