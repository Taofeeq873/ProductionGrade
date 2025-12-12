using Application.Commands.Store.ProductCommands;
using Application.Queries.Store.ProductQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var query = new GetProduct.GetProductQuery(id);
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(command, cancellationToken);
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProducts.GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }


    [HttpPut("{productId:guid}/update-quantity")]
    public async Task<IActionResult> UpdateProductQuantity([FromRoute] Guid productId,
        [FromBody] UpdateProductQuantity.UpdateProductQuantityCommand command)
    {
        command.ProductId = productId;
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var result = new DeleteProduct.DeleteProductCommand(id);
        var product = await _mediator.Send(result);

        return Ok(product);
    }
}