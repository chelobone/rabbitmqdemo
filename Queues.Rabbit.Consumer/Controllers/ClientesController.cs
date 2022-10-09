using Microsoft.AspNetCore.Mvc;
using Queues.Rabbit.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Queues.Rabbit.Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        // POST api/<ClientesController>
        [HttpPost]
        public void Post([FromBody] Client client)
        {
            new DAClient().ConnectionToDatabase(client);
        }
    }
}
