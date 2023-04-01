namespace EFO.WebUi.Pages;

public sealed class CatalogViewModelFactory : ICatalogViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CatalogViewModelFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public CatalogViewModel Create()
    {
        return _serviceProvider.GetRequiredService<CatalogViewModel>();
    }
}
