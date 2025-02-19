using System.Net;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProductById(string id)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(response);
    }

    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductByProductName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductName(string productName)
    {
        var response = await _mediator.Send(new GetProductByNameQuery(productName));
        return Ok(response);
    }

    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery]CatalogSpecParams catalogSpecParams)
    {
        var response = await _mediator.Send(new GetAllProductsQuery(catalogSpecParams));
        return Ok(response);
    }

    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
    {
        var response = await _mediator.Send(new GetAllBrandsQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("GetAllProductTypes")]
    [ProducesResponseType(typeof(IList<TypeResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<TypeResponse>>> GetAllProductTypes()
    {
        var response = await _mediator.Send(new GetAllTypesQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("[action]/{brand}", Name = "GetProductByProductBrandName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductBrandName(string brand)
    {
        var response = await _mediator.Send(new GetProductByBrandQuery(brand));
        return Ok(response);
    }

    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send<ProductResponse>(command);
        return Ok(result);
    }

    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}",Name = "DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteProduct(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        return Ok(result);
    }
}