using System.Threading.Tasks;
using APILogsRabbitMQ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace APILogsRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService personService)
        {
            _logService = personService;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> CreateAsync(Log log)
        {
            _logService.Create(log);

            return CreatedAtRoute("GetCliente", new { id = log.Id.ToString() }, log);
        }
    }
}
