using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Queues.Rabbit.Shared;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Queues.Rabbit.Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IBus _bus;

        public CustomerController(IBus bus)
        {
            _bus = bus;
        }


        // GET api/<CustomerController>/uniqueId
        [HttpGet("{uniqueId}")]
        public async Task<IActionResult> GetByName(string uniqueId)
        {
            var client = new DAClient().ObtenerRegistroCreado(uniqueId);
            if (client != null)
            {
                return Ok(new { ok = true, data = client });
            }

            return BadRequest(new { ok = false, data = "" });
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            if (client != null)
            {
                var offset = DateTimeOffset.Now;
                client.CreatedOn = offset.DateTime;
                client.uniqueId = Convert.ToBase64String(Encoding.ASCII.GetBytes(offset.ToUnixTimeSeconds().ToString()));
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(client);
                return Ok(new { ok = true, data = client.uniqueId });
            }
            return BadRequest(new { ok = false, data = "" });
        }
    }
}
