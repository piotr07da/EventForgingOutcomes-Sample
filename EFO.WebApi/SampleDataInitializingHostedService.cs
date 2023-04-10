// ReSharper disable InconsistentNaming

using EFO.Catalog.Application.Commands.Categories;
using EFO.Catalog.Application.Commands.ProductProperties;
using EFO.Catalog.Application.Commands.Products;
using EFO.Sales.Application.Commands.Customers;
using EFO.Sales.Application.Commands.Orders;
using EFO.Sales.Application.Commands.Products;
using MassTransit.Mediator;
using IntroduceProductInCatalog = EFO.Catalog.Application.Commands.Products.IntroduceProduct;
using IntroduceProductInSales = EFO.Sales.Application.Commands.Products.IntroduceProduct;

namespace EFO.WebApi;

internal sealed class SampleDataInitializingHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IMediator _mediator;

    public SampleDataInitializingHostedService(IHostApplicationLifetime lifetime, IMediator mediator)
    {
        _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _lifetime.ApplicationStarted.Register(OnStarted);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        Task.Run(async () =>
        {
            var customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var orderId = Guid.Parse("22222222-1111-1111-1111-111111111111");
            await PublishAsync(new RegisterCustomer(customerId));
            await PublishAsync(new StartOrder(orderId, customerId));

            var cSemiconductorsId = await AddCategoryAsync("Semiconductors");
            var cSemiconductorsTransistorsId = await AddSubcategoryAsync(cSemiconductorsId, "Transistors");
            var cSemiconductorsTransistorsUnipolarId = await AddSubcategoryAsync(cSemiconductorsTransistorsId, "Unipolar transistors");
            var cSemiconductorsTransistorsUnipolarNChannelId = await AddSubcategoryAsync(cSemiconductorsTransistorsUnipolarId, "N channel transistors");
            var cSemiconductorsTransistorsUnipolarPChannelId = await AddSubcategoryAsync(cSemiconductorsTransistorsUnipolarId, "P channel transistors");
            var cSemiconductorsDiodesId = await AddSubcategoryAsync(cSemiconductorsId, "Diodes");
            var cBridgeRectifiersId = await AddSubcategoryAsync(cSemiconductorsId, "Bridge rectifiers");
            var cIntegratedCircuitsId = await AddSubcategoryAsync(cSemiconductorsId, "Integrated circuits");
            var cDiacsId = await AddSubcategoryAsync(cSemiconductorsId, "Diacs");
            var cTriacsId = await AddSubcategoryAsync(cSemiconductorsId, "Triacs");

            var cPassivesId = await AddCategoryAsync("Passives");
            var cPassivesResistorsId = await AddSubcategoryAsync(cPassivesId, "Resistors");
            var cPassivesCapacitorsId = await AddSubcategoryAsync(cPassivesId, "Capacitors");
            var cPassivesInductorsId = await AddSubcategoryAsync(cPassivesId, "Inductors");
            var cPotentiometersId = await AddSubcategoryAsync(cPassivesId, "Potentiometers");
            var cEncodersId = await AddSubcategoryAsync(cPassivesId, "Encoders");

            var npDrainSourceVoltageId = await DefineNumericPropertyAsync("Drain-source voltage", "V");
            var npPowerId = await DefineNumericPropertyAsync("Power", "W");
            var npResistanceId = await DefineNumericPropertyAsync("Resistance", "Ohm");
            var npMaxOperVoltId = await DefineNumericPropertyAsync("Max. operating voltage", "V");
            var npManufacturerId = await DefineTextPropertyAsync("Manufacturer");
            var npMountingId = await DefineTextPropertyAsync("Mounting");

            var pTran10n65lgeId = await IntroduceProductAsync("10N65-LGE", cSemiconductorsTransistorsUnipolarNChannelId);
            await PublishAsync(new SetProductNumericProperty(pTran10n65lgeId, npDrainSourceVoltageId, 650));
            await PublishAsync(new SetProductTextProperty(pTran10n65lgeId, npManufacturerId, "LUGUANG ELECTRONIC"));
            await PublishAsync(new SetProductTextProperty(pTran10n65lgeId, npMountingId, "THT"));
            await PublishAsync(new PriceProduct(pTran10n65lgeId, 1, 3.54848m));
            await PublishAsync(new PriceProduct(pTran10n65lgeId, 5, 2.32869m));
            await PublishAsync(new PriceProduct(pTran10n65lgeId, 25, 2.09804m));
            await PublishAsync(new PriceProduct(pTran10n65lgeId, 100, 1.85408m));
            await PublishAsync(new PriceProduct(pTran10n65lgeId, 250, 1.66335m));

            var pTran2sj168Id = await IntroduceProductAsync("2SJ168", cSemiconductorsTransistorsUnipolarPChannelId);
            await PublishAsync(new SetProductNumericProperty(pTran2sj168Id, npDrainSourceVoltageId, -60));
            await PublishAsync(new SetProductTextProperty(pTran2sj168Id, npManufacturerId, "TOSHIBA"));
            await PublishAsync(new SetProductTextProperty(pTran2sj168Id, npMountingId, "SMD"));
            await PublishAsync(new PriceProduct(pTran2sj168Id, 1, 3.28m));
            await PublishAsync(new PriceProduct(pTran2sj168Id, 5, 2.39m));
            await PublishAsync(new PriceProduct(pTran2sj168Id, 25, 2.11m));
            await PublishAsync(new PriceProduct(pTran2sj168Id, 100, 1.92m));
            await PublishAsync(new PriceProduct(pTran2sj168Id, 500, 1.78m));

            var pResistor12w100rId = await IntroduceProductAsync("1/2W-100R", cPassivesResistorsId);
            await PublishAsync(new SetProductNumericProperty(pResistor12w100rId, npPowerId, .5m));
            await PublishAsync(new SetProductNumericProperty(pResistor12w100rId, npResistanceId, 100));
            await PublishAsync(new SetProductNumericProperty(pResistor12w100rId, npMaxOperVoltId, 350));
            await PublishAsync(new SetProductTextProperty(pResistor12w100rId, npManufacturerId, "ROYAL OHM"));
            await PublishAsync(new SetProductTextProperty(pResistor12w100rId, npMountingId, "THT"));
            await PublishAsync(new PriceProduct(pResistor12w100rId, 100, 0.17971m));
            await PublishAsync(new PriceProduct(pResistor12w100rId, 500, 0.12491m));
            await PublishAsync(new PriceProduct(pResistor12w100rId, 1000, 0.09350m));
            var pResistor12w10kId = await IntroduceProductAsync("1/2W-10K", cPassivesResistorsId);
            await PublishAsync(new SetProductNumericProperty(pResistor12w10kId, npPowerId, .5m));
            await PublishAsync(new SetProductNumericProperty(pResistor12w10kId, npResistanceId, 10000));
            await PublishAsync(new SetProductNumericProperty(pResistor12w10kId, npMaxOperVoltId, 350));
            await PublishAsync(new SetProductTextProperty(pResistor12w10kId, npManufacturerId, "ROYAL OHM"));
            await PublishAsync(new SetProductTextProperty(pResistor12w10kId, npMountingId, "THT"));
            await PublishAsync(new PriceProduct(pResistor12w10kId, 100, 0.17971m));
            await PublishAsync(new PriceProduct(pResistor12w10kId, 500, 0.12491m));
            await PublishAsync(new PriceProduct(pResistor12w10kId, 1000, 0.09350m));
            var pResistor5w10rId = await IntroduceProductAsync("5W-10R", cPassivesResistorsId);
            await PublishAsync(new SetProductNumericProperty(pResistor5w10rId, npPowerId, 5m));
            await PublishAsync(new SetProductNumericProperty(pResistor5w10rId, npResistanceId, 10));
            await PublishAsync(new SetProductNumericProperty(pResistor5w10rId, npMaxOperVoltId, 350));
            await PublishAsync(new SetProductTextProperty(pResistor5w10rId, npManufacturerId, "ROYAL OHM"));
            await PublishAsync(new SetProductTextProperty(pResistor5w10rId, npMountingId, "THT"));
            await PublishAsync(new PriceProduct(pResistor5w10rId, 5000, 0.70809m));
            var pResistor5w47rId = await IntroduceProductAsync("5W-47R", cPassivesResistorsId);
            await PublishAsync(new SetProductNumericProperty(pResistor5w47rId, npPowerId, 5m));
            await PublishAsync(new SetProductNumericProperty(pResistor5w47rId, npResistanceId, 47));
            await PublishAsync(new SetProductNumericProperty(pResistor5w47rId, npMaxOperVoltId, 350));
            await PublishAsync(new SetProductTextProperty(pResistor5w47rId, npManufacturerId, "ROYAL OHM"));
            await PublishAsync(new SetProductTextProperty(pResistor5w47rId, npMountingId, "THT"));
            await PublishAsync(new PriceProduct(pResistor5w47rId, 2, 2.08910m));
            await PublishAsync(new PriceProduct(pResistor5w47rId, 10, 1.34582m));
            await PublishAsync(new PriceProduct(pResistor5w47rId, 50, 0.85763m));
            await PublishAsync(new PriceProduct(pResistor5w47rId, 200, 0.79166m));
            await PublishAsync(new PriceProduct(pResistor5w47rId, 500, 0.74328m));
            await Task.CompletedTask;
        });
    }

    private async Task<Guid> AddCategoryAsync(string name)
    {
        var id = NewId();
        await PublishAsync(new AddCategory(id, name));
        return id;
    }

    private async Task<Guid> AddSubcategoryAsync(Guid parentId, string name)
    {
        var id = NewId();
        await PublishAsync(new AddSubcategory(parentId, id, name));
        return id;
    }

    private async Task<Guid> DefineNumericPropertyAsync(string name, string unit)
    {
        var id = NewId();
        await PublishAsync(new DefineNumericProperty(id, name, unit));
        return id;
    }

    private async Task<Guid> DefineTextPropertyAsync(string name)
    {
        var id = NewId();
        await PublishAsync(new DefineTextProperty(id, name));
        return id;
    }

    private async Task<Guid> IntroduceProductAsync(string name, Guid categoryId)
    {
        var id = NewId();
        await PublishAsync(new IntroduceProductInCatalog(id, name));
        await PublishAsync(new IntroduceProductInSales(id, name));
        await PublishAsync(new MoveProductToCategory(id, categoryId));
        return id;
    }

    private async Task PublishAsync(object command) => await _mediator.Publish(command);

    private static Guid NewId() => Guid.NewGuid();
}
