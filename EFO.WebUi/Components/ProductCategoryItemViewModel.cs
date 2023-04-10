using EFO.WebUi.Data;
using ReactiveUI;

namespace EFO.WebUi.Components;

public class ProductCategoryItemViewModel : ReactiveObject
{
    public ProductCategoryItemViewModel(ProductCategoryDto category)
    {
        Category = category;
    }

    public ProductCategoryDto Category { get; }
}
