using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace Ordering.API.Controllers;

public class OrderController : ApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IMediator mediator, ILogger<OrderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{userName}", Name = "GetOrdersByUserName")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
    {
        var orders = await _mediator.Send(new GetOrderListQuery(userName));
        return Ok(orders);
    }

    [HttpPost(Name = "CheckoutOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderResponse>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{userName}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        await _mediator.Send(new DeleteOrderCommand { Id = id });
        return NoContent();
    }
}