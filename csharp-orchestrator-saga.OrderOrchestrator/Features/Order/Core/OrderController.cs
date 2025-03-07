using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_orchestrator_saga.OrderOrchestrator.Features.Order.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderCommand command, CancellationToken cs)
        {
            var result = await _sender.Send(command, cs);
            return Ok(result);
        }
    }
}
