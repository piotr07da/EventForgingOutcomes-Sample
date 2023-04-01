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
        var response = await client.GetResponse<ProductCategoriesDto>(query, cancellationToken);
        return response.Message;
    }

    [HttpGet("{categoryId}")]
    public async Task<ProductCategoryDto> GetCategory([FromRoute] Guid categoryId, CancellationToken cancellationToken = default)
    {
        var query = new GetCategory(categoryId);

        var client = _mediator.CreateRequestClient<GetCategory>();
        var response = await client.GetResponse<ProductCategoryDto>(query, cancellationToken);
        return response.Message;
    }
}
