using EFO.SharedReadModel.Queries;
using EFO.SharedReadModel.ReadModel.Products;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EFO.WebApi.Controllers.ProductCategories;

[ApiController]
[Route("product-categories")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductCategoriesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("")]
    public async Task<ProductCategoriesDto> GetCategories([FromQuery] Guid? parentCategoryId, CancellationToken cancellationToken = default)
    {
        var query = new GetCategories(parentCategoryId);

        var client = _mediator.CreateRequestClient<GetCategories>();
        var orderResponse = await client.GetResponse<ProductCategoriesDto>(query, cancellationToken);
        return orderResponse.Message;
    }
}
