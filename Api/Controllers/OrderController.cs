using Application.Queries.OrderQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.OrderCommands;
using Application.Queries.OrderQueries;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrder.CreateOrderCommand request,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListOrders.ListOrdersQuery request,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetOrder.GetOrderQuery(id);
            var order = await _mediator.Send(query, cancellationToken);
            return Ok(order);
        }
    }
}